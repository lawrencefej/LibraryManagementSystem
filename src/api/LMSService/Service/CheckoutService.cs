using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class CheckoutService : BaseService<Checkout, CheckoutForDetailedDto, CheckoutForListDto, CheckoutService>, ICheckoutService
    {
        public CheckoutService(DataContext context, IMapper mapper, ILogger<CheckoutService> logger) : base(context, mapper, logger)
        {
        }

        public async Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckInAsset(CheckoutForCheckInDto checkoutForCheckIn)
        {
            LmsResponseHandler<Checkout> checkoutResult = await ValidateCheckIn(checkoutForCheckIn.CheckoutId);

            if (checkoutResult.Succeeded)
            {
                if (checkoutForCheckIn.IsRenew)
                {
                    checkoutResult.Item.RenewCheckout();
                }
                else
                {
                    checkoutResult.Item.CheckInAsset();
                }
                await Context.SaveChangesAsync();
                return LmsResponseHandler<CheckoutForDetailedDto>.Successful();
            }

            return LmsResponseHandler<CheckoutForDetailedDto>.Failed(checkoutResult.Errors);
        }

        public async Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckoutAssets(BasketForCheckoutDto basketForCheckout)
        {
            LmsResponseHandler<LibraryCard> cardResult = await ValidateCard(basketForCheckout.LibraryCardId);

            if (cardResult.Succeeded)
            {
                List<int> assetsForCheckoutIds = basketForCheckout.Assets.Select(item => item.LibraryAssetId)
                                                         .ToList();

                LmsResponseHandler<Checkout> checkoutResult = ValidateCheckout(cardResult.Item.Checkouts, assetsForCheckoutIds);

                if (checkoutResult.Succeeded)
                {

                    LmsResponseHandler<List<LibraryAsset>> assetResult = await ValidateAssets(assetsForCheckoutIds);

                    if (assetResult.Succeeded)
                    {
                        IList<Checkout> checkouts = assetResult.Item.Select(asset => new Checkout()
                        {
                            LibraryAssetId = asset.Id,
                            LibraryCardId = cardResult.Item.Id
                        }).ToList();

                        foreach (LibraryAsset asset in assetResult.Item)
                        {
                            asset.ReduceCopiesAvailable();
                        }
                        Context.AddRange(checkouts);
                        await Context.SaveChangesAsync();

                        return LmsResponseHandler<CheckoutForDetailedDto>.Successful();
                    }

                    return LmsResponseHandler<CheckoutForDetailedDto>.Failed(assetResult.Errors);
                }

                return LmsResponseHandler<CheckoutForDetailedDto>.Failed(checkoutResult.Errors);
            }

            return LmsResponseHandler<CheckoutForDetailedDto>.Failed(cardResult.Errors);
        }

        public async Task<LmsResponseHandler<CheckoutForDetailedDto>> GetCheckoutWithDetails(int checkoutId)
        {
            return MapDetailReturn(await GetCheckout(checkoutId));
        }

        public async Task<PagedList<CheckoutForListDto>> GetCheckoutsForAsset(int libraryAssetId, PaginationParams paginationParams)
        {
            IQueryable<Checkout> checkouts = Context.Checkouts.AsNoTracking()
                                                               .Include(a => a.LibraryAsset)
                                                               .Include(a => a.LibraryCard)
                                                               .Where(x => x.LibraryAssetId == libraryAssetId);

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetCheckoutsForCard(int LibraryCardId, PaginationParams paginationParams)
        {

            IQueryable<Checkout> checkouts = Context.Checkouts.AsNoTracking()
                                                               .Include(a => a.LibraryAsset)
                                                               .Include(a => a.LibraryCard)
                                                               .Where(l => l.LibraryCard.Id == LibraryCardId);

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetCheckouts(PaginationParams paginationParams)
        {
            IQueryable<Checkout> checkouts = Context.Checkouts.AsNoTracking()
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .AsQueryable();

            return await FilterCheckouts(paginationParams, checkouts);
        }

        private async Task<Checkout> GetCheckout(int checkoutId)
        {
            Checkout checkout = await Context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefaultAsync(a => a.Id == checkoutId);

            return checkout;
        }

        private async Task<LmsResponseHandler<Checkout>> ValidateCheckIn(int checkoutId)
        {
            Checkout checkout = await GetCheckout(checkoutId);

            if (checkout != null)
            {
                if (checkout.Status != CheckoutStatus.Returned)
                {
                    return LmsResponseHandler<Checkout>.Successful(checkout);
                }

                Logger.LogError($"{checkoutId} has already been returned");
                return LmsResponseHandler<Checkout>.Failed(new List<string>() { "Item has already been returned" });
            }

            Logger.LogError($"{checkoutId} was null");
            return LmsResponseHandler<Checkout>.Failed(new List<string>() { "Checkout does not exist" });
        }

        private async Task<LmsResponseHandler<List<LibraryAsset>>> ValidateAssets(List<int> ids)
        {
            List<LibraryAsset> assetsForCheckout = await Context.LibraryAssets
                                                        .Where(asset => ids.Contains(asset.Id))
                                                        .ToListAsync();

            if (assetsForCheckout.Any(r => r.Status == LibraryAssetStatus.Unavailable))
            {
                List<string> errors = new();

                foreach (LibraryAsset asset in assetsForCheckout)
                {
                    errors.Add($"Item '{asset.Title}' is not available at this time");
                }

                return LmsResponseHandler<List<LibraryAsset>>.Failed(errors);
            }

            return LmsResponseHandler<List<LibraryAsset>>.Successful(assetsForCheckout);
        }

        private async Task<PagedList<CheckoutForListDto>> FilterCheckouts(PaginationParams paginationParams, IQueryable<Checkout> checkouts)
        {
            if (string.Equals(paginationParams.SearchString, "returns", StringComparison.OrdinalIgnoreCase))
            {
                checkouts = checkouts.Where(x => x.Status == CheckoutStatus.Returned);
            }
            else if (string.Equals(paginationParams.SearchString, "checkouts", StringComparison.OrdinalIgnoreCase))
            {
                checkouts = checkouts.Where(x => x.Status == CheckoutStatus.Checkedout);
            }

            checkouts = paginationParams.SortDirection == "desc"
                ? string.Equals(paginationParams.OrderBy, "duedate", StringComparison.OrdinalIgnoreCase)
                    ? checkouts.OrderByDescending(x => x.DueDate)
                    : checkouts.OrderByDescending(x => x.CheckoutDate)
                : string.Equals(paginationParams.OrderBy, "duedate", StringComparison.OrdinalIgnoreCase)
                    ? checkouts.OrderBy(x => x.DueDate)
                    : checkouts.OrderBy(x => x.CheckoutDate);

            return await MapPagination(checkouts, paginationParams);
        }

        private async Task<LmsResponseHandler<LibraryCard>> ValidateCard(int libraryCardId)
        {
            LibraryCard card = await Context.LibraryCards.AsNoTracking()
                                    .Include(a => a.Checkouts
                                    .Where(d => d.Status == CheckoutStatus.Checkedout))
                                    .ThenInclude(item => item.LibraryAsset)
                                    .FirstOrDefaultAsync(c => c.Id == libraryCardId);

            return card != null
                ? card.HasFees()
                    ? LmsResponseHandler<LibraryCard>.Failed(new List<string>() { "This card has current fees" })
                    : LmsResponseHandler<LibraryCard>.Successful(card)
                : LmsResponseHandler<LibraryCard>.Failed(new List<string>() { "Selected card does not exist" });
        }

        private static LmsResponseHandler<Checkout> ValidateCheckout(ICollection<Checkout> currentCheckouts, List<int> ids)
        {
            IEnumerable<Checkout> conflictItems = currentCheckouts.Where(checkout => ids.Contains(checkout.LibraryAssetId));

            if ((currentCheckouts.Count + ids.Count) > 10)
            {
                return LmsResponseHandler<Checkout>.Failed(new List<string>() { $"{ids.Count} items puts the current checked out items past the maximum allowed of 10" });
            }

            if (conflictItems.Any())
            {
                List<string> errors = conflictItems.Select(item => $"Item '{item.LibraryAsset.Title}' has already been checkedout by this member").ToList();

                return LmsResponseHandler<Checkout>.Failed(errors);
            }

            return LmsResponseHandler<Checkout>.Successful();
        }
    }
}

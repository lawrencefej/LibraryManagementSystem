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
        private readonly DataContext _context;
        private readonly ILogger<CheckoutService> _logger;

        public CheckoutService(DataContext context, IMapper mapper, ILogger<CheckoutService> logger) : base(mapper)
        {
            _context = context;
            _logger = logger;
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
                await _context.SaveChangesAsync();
                return LmsResponseHandler<CheckoutForDetailedDto>.Successful();
            }

            return LmsResponseHandler<CheckoutForDetailedDto>.Failed(checkoutResult.Errors);
        }

        public async Task<LmsResponseHandler<CheckoutForDetailedDto>> CheckoutAssets(Basket basketForCheckout)
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
                        _context.AddRange(checkouts);
                        await _context.SaveChangesAsync();

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

        public async Task<Checkout> GetCheckout(int checkoutId)
        {
            Checkout checkout = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefaultAsync(a => a.Id == checkoutId);

            return checkout;
        }

        public async Task<PagedList<CheckoutForListDto>> GetCurrentCheckoutsForAsset(int libraryAssetId, PaginationParams paginationParams)
        {
            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                                                               .Include(a => a.LibraryCard)
                                                               .Where(l => l.Status == CheckoutStatus.Checkedout)
                                                               .Where(x => x.LibraryAssetId == libraryAssetId);

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetCheckoutHistoryForAsset(int libraryAssetId, PaginationParams paginationParams)
        {
            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                                                               .Where(l => l.Status == CheckoutStatus.Returned)
                                                               .Include(a => a.LibraryCard)
                                                               .Where(x => x.LibraryAssetId == libraryAssetId);

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetCurrentCheckoutsForCard(int LibraryCardId, PaginationParams paginationParams)
        {

            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                                                               .Include(a => a.LibraryAsset)
                                                               .Include(a => a.LibraryCard)
                                                               .Where(l => l.LibraryCard.Id == LibraryCardId)
                                                               .Where(l => l.Status == CheckoutStatus.Checkedout);

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetCheckoutHistoryForCard(int LibraryCardId, PaginationParams paginationParams)
        {

            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                                                               //    .Include(a => a.Items)
                                                               .Include(a => a.LibraryCard)
                                                               .Where(l => l.LibraryCard.Id == LibraryCardId)
                                                               .Where(l => l.Status == CheckoutStatus.Returned);

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetAllCurrentCheckouts(PaginationParams paginationParams)
        {
            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(x => x.Status == CheckoutStatus.Checkedout)
                .AsQueryable();

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public async Task<PagedList<CheckoutForListDto>> GetCheckoutHistory(PaginationParams paginationParams)
        {
            // TODO Make this into one filter
            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(x => x.Status == CheckoutStatus.Returned)
                .AsQueryable();

            return await FilterCheckouts(paginationParams, checkouts);
        }

        public Task<PagedList<CheckoutForListDto>> GetAllCheckoutsForMember(int userId)
        {
            throw new NotImplementedException();
        }

        private async Task<LmsResponseHandler<Checkout>> ValidateCheckIn(int checkoutId)
        {
            Checkout checkout = await _context.Checkouts.Include(a => a.LibraryAsset)
                                                        .FirstOrDefaultAsync(x => x.Id == checkoutId);

            if (checkout != null)
            {
                if (checkout.Status != CheckoutStatus.Returned)
                {
                    return LmsResponseHandler<Checkout>.Successful(checkout);
                }

                _logger.LogError($"{checkoutId} has already been returned");
                return LmsResponseHandler<Checkout>.Failed(new List<string>() { "Item has already been returned" });
            }

            _logger.LogError($"{checkoutId} was null");
            return LmsResponseHandler<Checkout>.Failed(new List<string>() { "Checkout does not exist" });
        }

        private async Task<LmsResponseHandler<List<LibraryAsset>>> ValidateAssets(List<int> ids)
        {
            List<LibraryAsset> assetsForCheckout = await _context.LibraryAssets
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
            checkouts = string.Equals(paginationParams.SearchString, "returned", StringComparison.OrdinalIgnoreCase)
                ? checkouts.Where(x => x.Status == CheckoutStatus.Returned)
                : checkouts.Where(x => x.Status == CheckoutStatus.Checkedout);

            checkouts = paginationParams.SortDirection == "desc"
                ? string.Equals(paginationParams.OrderBy, "until", StringComparison.OrdinalIgnoreCase)
                    ? checkouts.OrderByDescending(x => x.DueDate)
                    : checkouts.OrderByDescending(x => x.CheckoutDate)
                : string.Equals(paginationParams.OrderBy, "until", StringComparison.OrdinalIgnoreCase)
                    ? checkouts.OrderBy(x => x.DueDate)
                    : checkouts.OrderBy(x => x.CheckoutDate);

            return await MapPagination(checkouts, paginationParams);
        }

        private async Task<LmsResponseHandler<LibraryCard>> ValidateCard(int libraryCardId)
        {
            LibraryCard card = await _context.LibraryCards.AsNoTracking()
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

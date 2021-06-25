using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Enumerations;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSService.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly DataContext _context;
        private readonly ILogger<CheckoutService> _logger;
        private readonly IMapper _mapper;

        public CheckoutService(DataContext context, IMapper mapper, ILogger<CheckoutService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CheckInAsset(int checkoutId)
        {
            Checkout checkout = await ValidateCheckIn(checkoutId);

            checkout.Status = CheckoutStatus.Returned;

            checkout.IsReturned = true;

            checkout.DateReturned = DateTime.Today;

            // var libraryAsset = await GetLibraryAsset(checkout.LibraryAssetId);
            LibraryAsset libraryAsset = await GetLibraryAsset(0);

            IncreaseAssetCopiesAvailable(libraryAsset);

            await _context.SaveChangesAsync();

            return;
        }

        public async Task<CheckoutForReturnDto> CheckoutAsset(CheckoutForCreationDto checkoutForCreation)
        {
            LibraryCard libraryCard = await GetMemberLibraryCard(checkoutForCreation.UserId);
            DoesMemberHaveFees(libraryCard.Fees);

            IsAssetCurrentlyCheckedOutByMember(libraryCard.Checkouts.ToList(), checkoutForCreation.LibraryAssetId, 0);

            LibraryAsset libraryAsset = await GetLibraryAsset(checkoutForCreation.LibraryAssetId);

            // checkoutForCreation.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreation.LibraryCardId = libraryCard.Id;
            checkoutForCreation.Fees = libraryCard.Fees;

            ReduceAssetCopiesAvailable(libraryAsset);

            Checkout checkout = _mapper.Map<Checkout>(checkoutForCreation);
            checkout.Status = CheckoutStatus.Checkedout;

            _context.Add(checkout);
            await _context.SaveChangesAsync();

            CheckoutForReturnDto checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
            // checkoutToReturn.Status = CheckoutStatus.Checkedout;
            // checkoutToReturn.Status = nameof(StatusEnum.Checkedout);
            return checkoutToReturn;
        }

        public async Task CheckoutAsset(IEnumerable<CheckoutForCreationDto> checkoutsForCreation)
        {
            LibraryCard libraryCard = await GetMemberLibraryCard(checkoutsForCreation.First().UserId);
            DoesMemberHaveFees(libraryCard.Fees);

            foreach (CheckoutForCreationDto item in checkoutsForCreation)
            {
                IsAssetCurrentlyCheckedOutByMember(libraryCard.Checkouts.ToList(), item.LibraryAssetId, checkoutsForCreation.Count());
            }

            foreach (CheckoutForCreationDto item in checkoutsForCreation)
            {
                item.LibraryCardId = libraryCard.Id;
                IsAssetAvailable(item.Asset);
            }

            IEnumerable<Checkout> checkouts = _mapper.Map<IEnumerable<Checkout>>(checkoutsForCreation);

            _context.AddRange(checkouts);
            await _context.SaveChangesAsync();
        }

        public async Task<Checkout> GetCheckout(int checkoutId)
        {
            Checkout checkout = await _context.Checkouts
                .Include(a => a.Items)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == checkoutId);

            return checkout;
        }

        public async Task<IEnumerable<Checkout>> GetCheckoutsForAsset(int libraryAssetId)
        {
            List<Checkout> checkouts = await _context.Checkouts.AsNoTracking()
                .Where(l => l.Status == CheckoutStatus.Checkedout)
                .Where(x => x.Items.Any(a => a.LibraryAssetId == libraryAssetId))
                .ToListAsync();

            return checkouts;
        }

        public async Task<IEnumerable<Checkout>> GetCheckoutsForMember(int userId)
        {
            LibraryCard card = await GetMemberLibraryCard(userId);

            List<Checkout> checkouts = await _context.Checkouts.AsNoTracking()
                .Include(a => a.Items)
                // .Include(a => a.Status)
                .Where(l => l.LibraryCard.Id == card.Id)
                .Where(l => l.Status == CheckoutStatus.Checkedout)
                .ToListAsync();

            return checkouts;
        }

        public async Task<LibraryAsset> GetLibraryAsset(int id)
        {
            LibraryAsset asset = await _context.LibraryAssets
                .Include(s => s.Status)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null)
            {
                _logger.LogError("Library asset was null");
                throw new NoValuesFoundException("LibraryAsset does not exist");
            }

            return asset;
        }

        public async Task<LibraryCard> GetMemberLibraryCard(int userId)
        {
            LibraryCard card = await _context.LibraryCards
                .Include(x => x.Checkouts)
                .FirstOrDefaultAsync(x => x.CardNumber == userId.ToString());

            if (card == null)
            {
                _logger.LogError("Member has no library Card");
                throw new NoValuesFoundException("LibraryCard does not exist");
            }

            return card;
        }

        private async void IsAssetAvailable(LibraryAssetForDetailedDto asset)
        {
            LibraryAsset libraryAsset = await _context.LibraryAssets
                .Include(s => s.Status)
                .Where(x => x.Status == LibraryAssetStatus.Available)
                .FirstOrDefaultAsync(x => x.Id == asset.Id);

            if (libraryAsset == null)
            {
                throw new LMSValidationException($"{asset.Title} Is not available at this time, try again later");
            }

            ReduceAssetCopiesAvailable(libraryAsset);
        }

        private void DoesMemberHaveFees(decimal fees)
        {
            if (fees > 0)
            {
                _logger.LogError("This member still has fees to pay");
                throw new LMSValidationException("This member still has fees to pay");
            }
        }

        private void IsAssetCurrentlyCheckedOutByMember(List<Checkout> checkouts, int assetId, int newCheckoutCount)
        {
            checkouts = checkouts.Where(x => x.Status == CheckoutStatus.Checkedout).ToList();

            if (checkouts.Count >= 5)
            {
                // TODO move count to appsetting
                throw new LMSValidationException("This Member has reached the max amount of checkouts");
            }

            if ((checkouts.Count + newCheckoutCount) > 5)
            {
                throw new LMSValidationException($"{newCheckoutCount} checkouts puts this Member above the maximum amount of checkouts allowed");
            }

            // if (checkouts.Exists(x => x.LibraryAssetId == assetId))
            // {
            //     throw new LMSValidationException("This asset is currently checked out by this member");
            // }
            if (checkouts.Exists(x => x.Items.Any(t => t.LibraryAssetId == assetId)))
            {
                throw new LMSValidationException("This asset is currently checked out by this member");
            }
        }

        public void ReduceAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable--;

            if (asset.CopiesAvailable == 0)
            {
                asset.Status = LibraryAssetStatus.Unavailable;
            }
        }

        public void IncreaseAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable++;

            if (asset.Status == LibraryAssetStatus.Unavailable)
            {
                asset.Status = LibraryAssetStatus.Available;
            }
        }

        public async Task<IEnumerable<Checkout>> SearchCheckouts(string searchString)
        {
            IQueryable<Checkout> query = _context.Checkouts.AsNoTracking()
                .Include(s => s.LibraryCard)
                .Include(s => s.Items)
                .AsQueryable();

            // query = query.Where(s => s.LibraryAsset.Title.Contains(searchString));
            query = query.Where(s => s.Items.Any(t => t.LibraryAsset.Title.Contains(searchString)));

            List<Checkout> checkouts = await query.ToListAsync();

            return checkouts;
        }

        private async Task<Checkout> ValidateCheckIn(int checkoutId)
        {
            Checkout checkout = await _context.Checkouts
                .FirstOrDefaultAsync(x => x.Id == checkoutId);

            if (checkout == null)
            {
                _logger.LogError("Checkout was null");
                throw new NoValuesFoundException("Checkout does not exist");
            }

            if (checkout.Status == CheckoutStatus.Returned || checkout.IsReturned)
            {
                _logger.LogError("Checkout has already been returned");
                throw new LMSValidationException("Checkout has already been returned");
            }

            return checkout;
        }

        //public async Task<PagedList<Checkout>> GetAllCurrentCheckouts(PaginationParams paginationParams)
        //{
        //    var checkouts = _context.Checkouts.AsNoTracking()
        //        .Include(a => a.LibraryAsset)
        //        .Include(a => a.Status)
        //        .Where(x => x.StatusId == (int)StatusEnum.Checkedout)
        //        .AsQueryable();

        //    checkouts = checkouts.OrderByDescending(a => a.Since);

        //    return await PagedList<Checkout>.CreateAsync(checkouts, paginationParams.PageNumber, paginationParams.PageSize);
        //}

        public async Task<PagedList<Checkout>> GetAllCurrentCheckouts(PaginationParams paginationParams)
        {
            IQueryable<Checkout> checkouts = _context.Checkouts.AsNoTracking()
                .Include(a => a.Items)
                .Where(x => x.Status == CheckoutStatus.Checkedout)
                .AsQueryable();

            if (string.Equals(paginationParams.SearchString, "returned", StringComparison.CurrentCultureIgnoreCase))
            {
                checkouts = checkouts.Where(x => x.Status == CheckoutStatus.Returned);
            }
            else if (string.Equals(paginationParams.SearchString, "checkedout", StringComparison.CurrentCultureIgnoreCase))
            {
                checkouts = checkouts.Where(x => x.Status == CheckoutStatus.Checkedout);
            }

            if (paginationParams.SortDirection == "asc")
            {
                if (string.Equals(paginationParams.OrderBy, "since", StringComparison.CurrentCultureIgnoreCase))
                {
                    checkouts = checkouts.OrderBy(x => x.CheckoutDate);
                }
                else if (string.Equals(paginationParams.OrderBy, "until", StringComparison.CurrentCultureIgnoreCase))
                {
                    checkouts = checkouts.OrderBy(x => x.DueDate);
                }
            }
            else if (paginationParams.SortDirection == "desc")
            {
                if (string.Equals(paginationParams.OrderBy, "since", StringComparison.CurrentCultureIgnoreCase))
                {
                    checkouts = checkouts.OrderByDescending(x => x.CheckoutDate);
                }
                else if (string.Equals(paginationParams.OrderBy, "until", StringComparison.CurrentCultureIgnoreCase))
                {
                    checkouts = checkouts.OrderByDescending(x => x.DueDate);
                }
            }
            else
            {
                checkouts = checkouts.OrderByDescending(x => x.CheckoutDate);
            }

            return await PagedList<Checkout>.CreateAsync(checkouts, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}

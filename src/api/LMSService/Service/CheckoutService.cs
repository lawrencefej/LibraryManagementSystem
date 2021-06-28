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
            LibraryCard libraryCard = await GetMemberLibraryCard(checkoutForCreation.LibraryCardNumber);
            DoesMemberHaveFees(libraryCard.Fees);

            // IsAssetCurrentlyCheckedOutByMember(checkoutForCreation, libraryCard);

            LibraryAsset libraryAsset = await GetLibraryAsset(checkoutForCreation.LibraryCardId);

            // checkoutForCreation.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreation.LibraryCardId = libraryCard.Id;
            // checkoutForCreation.Fees = libraryCard.Fees;

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
            LibraryCard libraryCard = await GetMemberLibraryCard(checkoutsForCreation.First().LibraryCardNumber);
            DoesMemberHaveFees(libraryCard.Fees);

            foreach (CheckoutForCreationDto item in checkoutsForCreation)
            {
                // IsAssetCurrentlyCheckedOutByMember(libraryCard.Checkouts.ToList(), item);
            }

            foreach (CheckoutForCreationDto item in checkoutsForCreation)
            {
                item.LibraryCardId = libraryCard.Id;
                // IsAssetAvailable(item.Asset);
            }

            IEnumerable<Checkout> checkouts = _mapper.Map<IEnumerable<Checkout>>(checkoutsForCreation);

            _context.AddRange(checkouts);
            await _context.SaveChangesAsync();
        }

        public async Task<LmsResponseHandler<CheckoutForReturnDto>> CheckoutItems(LibraryCard card, CheckoutForCreationDto checkoutDto)
        {
            if (DoesMemberHaveFees(card.Fees))
            {
                return LmsResponseHandler<CheckoutForReturnDto>.Failed("This member still has fees to pay");
            }

            if ((card.Checkouts.Count + checkoutDto.Items.Count) > 5)
            {
                return LmsResponseHandler<CheckoutForReturnDto>.Failed($"{checkoutDto.Items.Count} checkouts puts this Member above the maximum amount of checkouts allowed");
            }

            if (checkoutDto.Items.Any(x => card.Checkouts.Any(y => y.LibraryCardId == x.LibraryAssetId)))
            {
                throw new LMSValidationException("This asset is currently checked out by this member");
            }

            // IsAssetCurrentlyCheckedOutByMember(libraryCard.Checkouts.ToList(), checkoutForCreation.LibraryAssetId, 0);

            // LibraryAsset libraryAsset = await GetLibraryAsset(checkoutForCreation.LibraryAssetId);
            IList<LibraryAsset> libraryAssets = await _context.LibraryAssets.Where(id => checkoutDto.Items.Any(a => a.LibraryAssetId == id.Id)).ToListAsync();

            // checkoutForCreation.AssetStatus = libraryAsset.Status.Name;
            // checkoutForCreation.LibraryCardId = libraryCard.Id;
            // checkoutForCreation.Fees = libraryCard.Fees;

            libraryAssets = ReduceAssetCopiesAvailable(libraryAssets);

            Checkout checkout = _mapper.Map<Checkout>(checkoutDto);
            checkout.Status = CheckoutStatus.Checkedout;

            _context.Add(checkout);
            await _context.SaveChangesAsync();

            CheckoutForReturnDto checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
            // checkoutToReturn.Status = CheckoutStatus.Checkedout;
            // checkoutToReturn.Status = nameof(StatusEnum.Checkedout);
            return LmsResponseHandler<CheckoutForReturnDto>.Successful(checkoutToReturn);

        }

        public async Task<Checkout> GetCheckout(int checkoutId)
        {
            Checkout checkout = await _context.Checkouts
                .Include(a => a.Items)
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

        public async Task<IEnumerable<Checkout>> GetCheckoutsForMember(string userId)
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

        public async Task<LibraryCard> GetMemberLibraryCard(string CardNumber)
        {
            LibraryCard card = await _context.LibraryCards
                .Include(x => x.Checkouts)
                .FirstOrDefaultAsync(x => x.CardNumber == CardNumber);

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

        // private void DoesMemberHaveFees(decimal fees)
        // {
        //     // TODO Return boolean and fail with an error
        //     if (fees > 0)
        //     {
        //         _logger.LogError("This member still has fees to pay");
        //         throw new LMSValidationException("This member still has fees to pay");
        //     }
        // }
        private bool DoesMemberHaveFees(decimal fees)
        {
            // TODO Return boolean and fail with an error
            if (fees > 0)
            {
                _logger.LogError("This member still has fees to pay");
                throw new LMSValidationException("This member still has fees to pay");
            }

            return fees > 0;
        }

        // private static void IsAssetCurrentlyCheckedOutByMember(CheckoutForCreationDto checkout, LibraryCard card)
        // {
        //     List<Checkout> currentCheckedoutItems = card.Checkouts.Where(x => x.Status == CheckoutStatus.Checkedout).ToList();

        //     if (checkout.Items.Count >= 5)
        //     {
        //         // TODO move count to appsetting
        //         throw new LMSValidationException("This Member has reached the max amount of checkouts");
        //     }

        //     if ((currentCheckedoutItems.Count + checkout.Items.Count) > 5)
        //     {
        //         throw new LMSValidationException($"{checkout.Items.Count} checkouts puts this Member above the maximum amount of checkouts allowed");
        //     }

        //     if (checkout.Items.Any(x => currentCheckedoutItems.Any(y => y.LibraryCardId == x.LibraryAssetId)))
        //     {
        //         throw new LMSValidationException("This asset is currently checked out by this member");
        //     }

        //     // if (checkouts.Exists(x => x.LibraryAssetId == assetId))
        //     // {
        //     //     throw new LMSValidationException("This asset is currently checked out by this member");
        //     // }
        //     // if (checkout.Exists(x => x.Items.Any(t => t.LibraryAssetId == assetId)))
        //     // {
        //     //     throw new LMSValidationException("This asset is currently checked out by this member");
        //     // }
        // }

        // private static TestResponseHandler<CheckoutForReturnDto> IsAssetCurrentlyCheckedOutByMember(CheckoutForCreationDto checkout, LibraryCard card)
        // {
        //     List<Checkout> currentCheckedoutItems = card.Checkouts.Where(x => x.Status == CheckoutStatus.Checkedout).ToList();

        //     if (checkout.Items.Count >= 5)
        //     {
        //         // TODO move count to appsetting
        //         throw new LMSValidationException("This Member has reached the max amount of checkouts");
        //     }

        //     if ((currentCheckedoutItems.Count + checkout.Items.Count) > 5)
        //     {
        //         throw new LMSValidationException($"{checkout.Items.Count} checkouts puts this Member above the maximum amount of checkouts allowed");
        //     }

        //     if (checkout.Items.Any(x => currentCheckedoutItems.Any(y => y.LibraryCardId == x.LibraryAssetId)))
        //     {
        //         throw new LMSValidationException("This asset is currently checked out by this member");
        //     }

        //     // if (checkouts.Exists(x => x.LibraryAssetId == assetId))
        //     // {
        //     //     throw new LMSValidationException("This asset is currently checked out by this member");
        //     // }
        //     // if (checkout.Exists(x => x.Items.Any(t => t.LibraryAssetId == assetId)))
        //     // {
        //     //     throw new LMSValidationException("This asset is currently checked out by this member");
        //     // }
        // }

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

        public static IList<LibraryAsset> ReduceAssetCopiesAvailable(IList<LibraryAsset> assets)
        {
            foreach (LibraryAsset item in assets)
            {
                item.CopiesAvailable--;

                if (item.CopiesAvailable == 0)
                {
                    item.Status = LibraryAssetStatus.Unavailable;
                }
            }

            return assets;
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

        public Task<IEnumerable<Checkout>> GetCheckoutsForMember(int userId)
        {
            throw new NotImplementedException();
        }
    }
}

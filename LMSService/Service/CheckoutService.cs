using AutoMapper;
using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Exceptions;
using LMSService.Helpers;
using LMSService.Interfaces;
using LMSService.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly DataContext _context;
        private readonly ILogger<CheckoutService> _logger;
        private readonly IMapper _mapper;
        private List<string> errors = new List<string>();

        public CheckoutService(DataContext context, IMapper mapper, ILogger<CheckoutService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CheckInAsset(int checkoutId)
        {
            var checkout = await ValidateCheckin(checkoutId);

            checkout.StatusId = (int)EnumStatus.Returned;

            checkout.IsReturned = true;

            checkout.DateReturned = DateTime.Today;

            var libraryAsset = await GetLibraryAsset(checkout.LibraryAssetId);

            libraryAsset.CopiesAvailable++;

            await _context.SaveChangesAsync();

            return;
        }

        public async Task<ResponseHandler> CheckoutAsset(CheckoutForCreationDto checkoutForCreation)
        {
            // TODO Fix this
            var libraryCard = await GetMemberLibraryCard(checkoutForCreation.userId);

            await IsAssetCurrentlyCheckedOutByMember(checkoutForCreation.LibraryAssetId, libraryCard.Id);

            var libraryAsset = await GetLibraryAsset(checkoutForCreation.LibraryAssetId);

            checkoutForCreation.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreation.LibraryCardId = libraryCard.Id;
            checkoutForCreation.Fees = libraryCard.Fees;
            // TODO Create helper for this
            checkoutForCreation.CurrentCheckoutCount = await GetMemberCurrentCheckoutAmount(libraryCard.Id);

            var validate = new CheckoutValidation();
            var result = await validate.ValidateAsync(checkoutForCreation);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors.Add($"{failure.ErrorMessage}");
                }
                return new ResponseHandler(errors);
            }

            ReduceAssetCopiesAvailable(libraryAsset);

            var checkout = _mapper.Map<Checkout>(checkoutForCreation);
            checkout.StatusId = (int)EnumStatus.Checkedout;

            _context.Add(checkout);
            await _context.SaveChangesAsync();

            var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
            checkoutToReturn.Status = nameof(EnumStatus.Checkedout);
            return new ResponseHandler(checkoutToReturn, checkoutToReturn.Id);
        }

        public async Task<int> GetMemberCurrentCheckoutAmount(int cardId)
        {
            var count = await _context.Checkouts
                .Where(l => l.LibraryCard.Id == cardId)
                .Where(l => l.StatusId == (int)EnumStatus.Checkedout)
                .CountAsync();

            return count;
        }

        public async Task<CheckoutForReturnDto> GetCheckout(int checkoutId)
        {
            var checkout = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == checkoutId);

            if (checkout == null)
            {
                throw new NoValuesFoundException("This checkout does not exist");
            }

            // TODO move mapper to the controller
            return _mapper.Map<CheckoutForReturnDto>(checkout);
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForAsset(int libraryAssetId)
        {
            var checkouts = await _context.Checkouts
                .Include(a => a.Status)
                .Where(l => l.LibraryAssetId == libraryAssetId)
                .Where(l => l.StatusId == (int)EnumStatus.Checkedout)
                .ToListAsync();

            // TODO move mapper to the controller
            return _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> GetCheckoutsForMember(int userId)
        {
            var card = await GetMemberLibraryCard(userId);

            var checkouts = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .Where(l => l.LibraryCard.Id == card.Id)
                .Where(l => l.DateReturned == null)
                .ToListAsync();
            // TODO move mapper to the controller
            return _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);
        }

        public async Task<LibraryAsset> GetLibraryAsset(int id)
        {
            var asset = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(p => p.Category)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
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
            var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.UserId == userId);

            if (card == null)
            {
                _logger.LogError("Member has no library Card");
                throw new NoValuesFoundException("LibraryCard does not exist");
            }

            return card;
        }

        public void ReduceAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable--;

            if (asset.CopiesAvailable == 0)
            {
                asset.StatusId = (int)EnumStatus.Unavailable;
            }
        }

        public async Task<IEnumerable<CheckoutForReturnDto>> SearchCheckouts(string searchString)
        {
            var query = _context.Checkouts
                .Include(s => s.LibraryCard)
                .Include(s => s.LibraryAsset)
                .Include(s => s.Status)
                .AsQueryable();

            query = query.Where(s => s.LibraryAsset.Title.Contains(searchString));

            var checkouts = await query.ToListAsync();

            // TODO move mapper to the controller
            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return checkoutsToReturn;
        }

        private async Task IsAssetCurrentlyCheckedOutByMember(int assetId, int cardId)
        {
            var checkout = await _context.Checkouts.
                Where(u => u.Status.Name == EnumStatus.Checkedout.ToString())
                .Where(u => u.LibraryCardId == cardId)
                .FirstOrDefaultAsync(u => u.LibraryAssetId == assetId);

            if (checkout != null)
            {
                throw new LMSValidationException("This asset is currently checked out by this member");
            }
        }

        private async Task<Checkout> ValidateCheckin(int checkoutId)
        {
            var checkout = await _context.Checkouts
                .FirstOrDefaultAsync(x => x.Id == checkoutId);

            if (checkout == null)
            {
                _logger.LogError("Checkout was null");
                throw new NoValuesFoundException("Checkout does not exist");
            }

            if (checkout.StatusId == (int)EnumStatus.Returned || checkout.IsReturned)
            {
                _logger.LogError("Checkout has already been returned");
                throw new LMSValidationException("Checkout has already been returned");
            }

            return checkout;
        }

        public async Task<PagedList<Checkout>> GetAllCheckouts(PaginationParams paginationParams)
        {
            var checkouts = _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .AsQueryable();

            checkouts = checkouts.OrderByDescending(a => a.Since);

            return await PagedList<Checkout>.CreateAsync(checkouts, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
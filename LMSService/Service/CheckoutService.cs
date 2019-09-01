using AutoMapper;
using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Exceptions;
using LMSService.Interfaces;
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

            IncreaseAssetCopiesAvailable(libraryAsset);

            await _context.SaveChangesAsync();

            return;
        }

        public async Task<CheckoutForReturnDto> CheckoutAsset(CheckoutForCreationDto checkoutForCreation)
        {
            var libraryCard = await GetMemberLibraryCard(checkoutForCreation.userId);
            DoesMemberHaveFees(libraryCard.Fees);

            IsAssetCurrentlyCheckedOutByMember(libraryCard.Checkouts.ToList(), checkoutForCreation.LibraryAssetId);

            var libraryAsset = await GetLibraryAsset(checkoutForCreation.LibraryAssetId);

            checkoutForCreation.AssetStatus = libraryAsset.Status.Name;
            checkoutForCreation.LibraryCardId = libraryCard.Id;
            checkoutForCreation.Fees = libraryCard.Fees;

            ReduceAssetCopiesAvailable(libraryAsset);

            var checkout = _mapper.Map<Checkout>(checkoutForCreation);
            checkout.StatusId = (int)EnumStatus.Checkedout;

            _context.Add(checkout);
            await _context.SaveChangesAsync();

            var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);
            checkoutToReturn.Status = nameof(EnumStatus.Checkedout);
            return checkoutToReturn;
        }

        public async Task<Checkout> GetCheckout(int checkoutId)
        {
            var checkout = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == checkoutId);

            return checkout;
        }

        public async Task<IEnumerable<Checkout>> GetCheckoutsForAsset(int libraryAssetId)
        {
            var checkouts = await _context.Checkouts
                .Include(a => a.Status)
                .Where(l => l.LibraryAssetId == libraryAssetId)
                .Where(l => l.StatusId == (int)EnumStatus.Checkedout)
                .ToListAsync();

            return checkouts;
        }

        public async Task<IEnumerable<Checkout>> GetCheckoutsForMember(int userId)
        {
            var card = await GetMemberLibraryCard(userId);

            var checkouts = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .Where(l => l.LibraryCard.Id == card.Id)
                .Where(l => l.DateReturned == null)
                .ToListAsync();

            return checkouts;
        }

        public async Task<LibraryAsset> GetLibraryAsset(int id)
        {
            var asset = await _context.LibraryAssets
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
            var card = await _context.LibraryCards
                .Include(x => x.Checkouts)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (card == null)
            {
                _logger.LogError("Member has no library Card");
                throw new NoValuesFoundException("LibraryCard does not exist");
            }

            return card;
        }

        private void DoesMemberHaveFees(decimal fees)
        {
            if (fees > 0)
            {
                _logger.LogError("This member still has fees to pay");
                throw new LMSValidationException("This member still has fees to pay");
            }
        }

        private void IsAssetCurrentlyCheckedOutByMember(List<Checkout> checkouts, int assetId)
        {
            checkouts = checkouts.Where(x => x.StatusId == (int)EnumStatus.Checkedout).ToList();

            if (checkouts.Count > 5)
            {
                // TODO move count to appsetting
                throw new LMSValidationException("This Member has reached the max amount of checkouts");
            }

            if (checkouts.Exists(x => x.LibraryAssetId == assetId))
            {
                throw new LMSValidationException("This asset is currently checked out by this member");
            }
        }

        public void ReduceAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable--;

            if (asset.CopiesAvailable == 0)
            {
                asset.StatusId = (int)EnumStatus.Unavailable;
            }
        }

        public void IncreaseAssetCopiesAvailable(LibraryAsset asset)
        {
            asset.CopiesAvailable++;

            if (asset.StatusId == (int)EnumStatus.Unavailable)
            {
                asset.StatusId = (int)EnumStatus.Available;
            }
        }

        public async Task<IEnumerable<Checkout>> SearchCheckouts(string searchString)
        {
            var query = _context.Checkouts
                .Include(s => s.LibraryCard)
                .Include(s => s.LibraryAsset)
                .Include(s => s.Status)
                .AsQueryable();

            query = query.Where(s => s.LibraryAsset.Title.Contains(searchString));

            var checkouts = await query.ToListAsync();

            return checkouts;
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
                .Where(x => x.StatusId == (int)EnumStatus.Checkedout)
                .AsQueryable();

            checkouts = checkouts.OrderByDescending(a => a.Since);

            return await PagedList<Checkout>.CreateAsync(checkouts, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
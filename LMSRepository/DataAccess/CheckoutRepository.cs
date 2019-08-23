using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class CheckoutRepository /*: ICheckoutRepository*/
    {
        private readonly DataContext _context;

        public CheckoutRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Checkout>> GetAllCheckouts()
        {
            var checkouts = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status).ToListAsync();

            return checkouts;
        }

        public async Task<Checkout> GetCheckout(int id)
        {
            var checkout = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == id);

            return checkout;
        }

        public async Task<List<Checkout>> GetCheckoutHistory(int cardId)
        {
            var checkoutHistory = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(l => l.LibraryCard.Id == cardId)
                .ToListAsync();

            return checkoutHistory;
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

        public async Task<IEnumerable<Checkout>> GetCheckoutsForMember(int cardId)
        {
            var checkouts = await _context.Checkouts
                .Where(l => l.LibraryCard.Id == cardId)
                .ToListAsync();

            return checkouts;
        }

        public Task<Checkout> GetLatestCheckout(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetMemberCurrentCheckoutAmount(int cardId)
        {
            var count = await _context.Checkouts
                .Where(l => l.LibraryCard.Id == cardId)
                .Where(l => l.StatusId == (int)EnumStatus.Checkedout)
                .CountAsync();

            return count;
        }

        public async Task<IEnumerable<Checkout>> GetMemberCurrentCheckouts(int cardId)
        {
            var checkout = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .Where(l => l.LibraryCard.Id == cardId)
                .Where(l => l.DateReturned == null)
                .ToListAsync();

            return checkout;
        }

        public async Task<bool> IsAssetCurrentlyCheckedOutByMember(int assetId, int cardId)
        {
            var checkout = await _context.Checkouts.
                Where(u => u.Status.Name == EnumStatus.Checkedout.ToString())
                .Where(u => u.LibraryCardId == cardId)
                .FirstOrDefaultAsync(u => u.LibraryAssetId == assetId);

            if (checkout == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<Checkout>> SearchCheckouts(string searchString)
        {
            var checkouts = from checkout in _context.Checkouts
                        .Include(s => s.LibraryCard)
                        .Include(s => s.LibraryAsset)
                        .Include(s => s.Status)
                            select checkout;

            if (!string.IsNullOrEmpty(searchString))
            {
                checkouts = checkouts
                    .Where(s => s.LibraryAsset.Title.Contains(searchString));

                return await checkouts.ToListAsync();
            }

            return await GetAllCheckouts();
        }

        public IQueryable<Checkout> GetAll()
        {
            var checkouts = _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.Status)
                .AsQueryable();

            return checkouts;
        }
    }
}
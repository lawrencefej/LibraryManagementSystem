using LMSLibrary.Data;
using LMSLibrary.Models;
using LMSRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class CheckoutRepository : ICheckoutRepository
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

        public async Task<IEnumerable<Checkout>> GetCheckoutsForMember(int id)
        {
            var checkouts = await _context.Checkouts
                .Where(l => l.LibraryCard.Id == id)
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
                .Where(l => l.Status.Name == "Checkedout")
                .CountAsync();

            return count;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSLibrary.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int id)
        {
            var query = _context.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .AsQueryable();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<List<Checkout>> GetUserCheckoutHistory(int id)
        {
            var checkoutHistory = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(l => l.LibraryCard.Id == id)
                .ToListAsync();

            return checkoutHistory;
        }

        public async Task<List<Checkout>> GetUserCurrentCheckouts(int id)
        {
            var userCheckouts = await GetUserCheckoutHistory(id);

            var currentCheckouts = userCheckouts.Where(u => u.IsReturned == false && u.DateReturned == null).ToList();

            return currentCheckouts;
        }

        public async Task<List<ReserveAsset>> GetUserCurrentReservedAssets(int id)
        {
            var userReserves = await GetUserReservedAssets(id);

            var currentReserves = userReserves.Where(u => u.IsCheckedOut == false && u.DateCheckedOut == null).ToList();

            return currentReserves;
        }

        public async Task<LibraryCard> GetUserLibraryCard(int id)
        {
            var libraryCard = await _context.LibraryCards
                .FirstOrDefaultAsync(c => c.UserId == id);

            return libraryCard;
        }

        public async Task<List<ReserveAsset>> GetUserReservedAssets(int id)
        {
            var reserveHistory = await _context.ReserveAssets
                .Include(a => a.LibraryAsset)
                .Where(l => l.LibraryCard.Id == id)
                .ToListAsync();

            return reserveHistory;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .OrderBy(u => u.Lastname).ToListAsync();

            return  users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

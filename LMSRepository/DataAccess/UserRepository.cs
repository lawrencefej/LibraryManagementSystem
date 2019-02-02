using LMSLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
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

        public async Task<User> GetUserByCardId(int cardId)
        {
            var user = await _context.Users
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.LibraryCard.CardNumber == cardId);

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email);

            return user;
        }

        public async Task<IEnumerable<Checkout>> GetUserCheckoutHistory(int memberId)
        {
            var checkoutHistory = await _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(l => l.LibraryCard.Id == memberId)
                .ToListAsync();

            return checkoutHistory;
        }

        public async Task<IEnumerable<Checkout>> GetUserCurrentCheckouts(int id)
        {
            var userCheckouts = await GetUserCheckoutHistory(id);

            var currentCheckouts = userCheckouts.Where(u => !u.IsReturned && u.DateReturned == null).ToList();

            return currentCheckouts;
        }

        public async Task<IEnumerable<ReserveAsset>> GetUserCurrentReservedAssets(int id)
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

        public async Task<IEnumerable<ReserveAsset>> GetUserReservedAssets(int memberId)
        {
            var reserveHistory = await _context.ReserveAssets
                .Include(a => a.LibraryAsset)
                .Where(l => l.LibraryCard.Id == memberId)
                .ToListAsync();

            return reserveHistory;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .OrderBy(u => u.Lastname).ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
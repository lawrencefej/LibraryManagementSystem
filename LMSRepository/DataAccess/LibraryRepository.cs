using LMSRepository.Data;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class LibraryRepository /*: ILibraryRepository*/
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public LibraryRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IList<User>> GetAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("admin");

            return admins;
        }

        public async Task<Checkout> GetCheckout(int id)
        {
            var checkout = await _context.Checkouts
                .Include(s => s.Status)
                .FirstOrDefaultAsync(a => a.Id == id);

            return checkout;
        }

        public async Task<List<Checkout>> GetCheckouts()
        {
            var checkouts = await _context.Checkouts
                .Include(s => s.Status)
                .Where(u => u.IsReturned == false).ToListAsync();

            return checkouts;
        }

        public async Task<List<Checkout>> GetCheckoutsForAsset(int id)
        {
            var asset = await _context.Checkouts
                .Where(a => a.LibraryAsset.Id == id).ToListAsync();

            return asset;
        }

        public async Task<List<Checkout>> GetCheckoutsHistory()
        {
            var checkouts = await _context.Checkouts.
                Where(u => u.IsReturned == true).ToListAsync();

            return checkouts;
        }

        public async Task<IList<User>> GetLibrarians()
        {
            var librarian = await _userManager.GetUsersInRoleAsync("librarian");

            return librarian;
        }

        public async Task<IList<User>> GetMembers()
        {
            var members = await _userManager.GetUsersInRoleAsync("member");

            return members;
        }

        public async Task<ReserveAsset> GetReserve(int id)
        {
            var reserve = await _context.ReserveAssets
                .Include(s => s.Status)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reserve;
        }

        public async Task<List<ReserveAsset>> GetReserves()
        {
            var reserves = await _context.ReserveAssets
                .Include(s => s.Status)
                .Where(u => u.IsCheckedOut == false).ToListAsync();

            return reserves;
        }

        public async Task<List<ReserveAsset>> GetReservesForAsset(int id)
        {
            var reserves = await _context.ReserveAssets
                .Include(s => s.Status)
                .Where(a => a.LibraryAsset.Id == id).ToListAsync();

            return reserves;
        }

        public async Task<List<ReserveAsset>> GetReservesHistory()
        {
            var reserves = await _context.ReserveAssets
                .Include(s => s.Status)
                .Where(u => u.IsCheckedOut == false || u.IsExpired == true).ToListAsync();

            return reserves;
        }

        public async Task<User> GetUser(int id)
        {
            var query = _context.Users
                .Include(c => c.LibraryCard)
                .AsQueryable();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<Status> GetStatus(string status)
        {
            return await _context.Statuses.FirstOrDefaultAsync(s => s.Name == status);
        }
    }
}
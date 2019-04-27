using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> SearchUsers(SearchUserDto searchUser)
        {
            var users = from user in _userManager.Users
                        .Include(s => s.LibraryCard)
                        .Where(u => u.UserRoles.Any(r => r.Role.Name == EnumRoles.Member.ToString()))
                        select user;

            users = users
                .Where(s => s.Email == searchUser.Email
            || s.FirstName.Contains(searchUser.FirstName)
            || s.Lastname.Contains(searchUser.LastName));

            return await users.ToListAsync();
        }

        public async Task<User> GetUserByCardId(int? cardId)
        {
            var user = await _userManager.Users
                .Include(c => c.LibraryCard)
                .Include(c => c.ProfilePicture)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.LibraryCard.Id == cardId);

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            // TODO try to optimize to one db trip
            var users = await _userManager.GetUsersInRoleAsync(nameof(EnumRoles.Member));

            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                user = await _userManager.Users
                    .Include(c => c.LibraryCard)
                    .Include(c => c.ProfilePicture)
                    .Include(c => c.UserRoles)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
            }

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

        public async Task<IEnumerable<User>> SearchUsers(string searchString)
        {
            var users = from user in _userManager.Users
                        .Include(s => s.LibraryCard)
                        .Include(s => s.UserRoles)
                        .Where(u => u.UserRoles.Any(r => r.Role.Name == EnumRoles.Member.ToString()))
                        select user;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users
                    .Where(s => s.Email.Contains(searchString)
                || s.FirstName.Contains(searchString)
                || s.Lastname.Contains(searchString)
                || Convert.ToString(s.LibraryCard.Id).Contains(searchString));

                return await users.ToListAsync();
            }

            return await GetUsers();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name == EnumRoles.Member.ToString()))
                .OrderBy(u => u.Lastname).ToListAsync();

            return users;
        }

        public async Task<IEnumerable<User>> GetAdmins()
        {
            var users = await _userManager.Users.Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name != EnumRoles.Member.ToString()))
                .OrderBy(u => u.Lastname).ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
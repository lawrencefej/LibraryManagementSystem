using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AdminRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> GetAdminUser(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(r => r.Role.Name != (nameof(EnumRoles.Member))))
                .OrderBy(u => u.Lastname).ToListAsync();

            return users;
        }

        public async Task CreateUser(User user, string password, string role)
        {
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task CreateUser(User user, string role)
        {
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task UpdateUser(User user, string newRole)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, newRole);

            if (!isInRole)
            {
                var roles = new List<string>()
                {
                    EnumRoles.Admin.ToString(),
                    EnumRoles.Librarian.ToString()
                };
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, newRole);
            }

            await _userManager.UpdateAsync(user);
        }

        public Task DeleteUser(User user, string newRole)
        {
            throw new System.NotImplementedException();
        }
    }
}
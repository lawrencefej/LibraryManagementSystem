using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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

        public Task<User> CreateAdminUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAdminUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAdminUsers()
        {
            var users = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role)
                //.Where(u => u.UserRoles.Any(r => r.Role.Name. == (nameof(EnumRoles.Admin) || nameof(EnumRoles.Librarian))))
                .Where(u => u.UserRoles.Any(r => r.Role.Name != (nameof(EnumRoles.Member))))
                .OrderBy(u => u.Lastname).ToListAsync();

            return users;
        }
    }
}
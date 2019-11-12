using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user;
        }

        public void AddRoleToUserDto(UserForDetailedDto user)
        {
            var role = user.UserRoles.ElementAtOrDefault(0);
            user.Role = role.Name;
        }

        public async Task UpdateUser(User user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}
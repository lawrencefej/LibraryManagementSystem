using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LMSService.Service
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdminService> _logger;
        private readonly DataContext _context;

        public AdminService(DataContext context, UserManager<User> userManager,
            IEmailSender emailSender, ILogger<AdminService> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        public async Task<IdentityResult> CreateUser(User user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<User> CompleteUserCreation(User newUser, string newRole, string callbackUrl)
        {
            await _userManager.AddToRoleAsync(newUser, newRole);

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(newUser);

            await WelcomeMessage(resetPasswordToken, newUser, callbackUrl);

            return newUser;
        }

        public UserForDetailedDto AddRoleToUser(UserForDetailedDto user)
        {
            var role = user.UserRoles.ElementAtOrDefault(0);

            user.Role = role.Name;

            return user;
        }

        public IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users)
        {
            foreach (var user in users)
            {
                var role = user.UserRoles.ElementAtOrDefault(0);
                user.Role = role.Name;
            }

            return users;
        }

        public async Task<User> GetAdminUser(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<IEnumerable<User>> GetAdminUsers()
        {
            var users = await _userManager.Users.AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(r => r.Role.Name != (nameof(EnumRoles.Member))))
                .OrderBy(u => u.FirstName).ToListAsync();

            return users;
        }

        public async Task UpdateUser(User userforUpdate, string role)
        {
            var isInRole = await _userManager.IsInRoleAsync(userforUpdate, role);

            if (!isInRole)
            {
                var roles = new List<string>()
                {
                    nameof(EnumRoles.Admin),
                    nameof(EnumRoles.Librarian)
                };
                await _userManager.RemoveFromRolesAsync(userforUpdate, roles);
                await _userManager.AddToRoleAsync(userforUpdate, role);
            }

            await _userManager.UpdateAsync(userforUpdate);
            await _context.SaveChangesAsync();
        }

        private async Task WelcomeMessage(string code, User user, string url)
        {
            var encodedToken = HttpUtility.UrlEncode(code);

            var callbackUrl = new Uri(url + user.Id + "/" + encodedToken);

            var body = $"Welcome {user.FirstName.ToLower()}, <p>An account has been created for you</p> Please create your new password by clicking <a href='{callbackUrl}'>here</a>:";

            await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        }

        public async Task DeleteUser(User user)
        {
            _context.Remove(user);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"userID: {user.Id} was deleted");
        }
    }
}
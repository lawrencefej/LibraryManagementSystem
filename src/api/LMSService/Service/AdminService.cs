using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSRepository.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdminService> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AdminService(DataContext context, UserManager<AppUser> userManager,
            IEmailSender emailSender, ILogger<AdminService> logger, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        public async Task<LmsResponseHandler<UserForDetailedDto>> CreateUser(AddAdminDto addAdminDto)
        {

            AppUser newUser = _mapper.Map<AppUser>(addAdminDto);
            newUser.UserName = newUser.Email.ToLower();
            IdentityResult result = await _userManager.CreateAsync(newUser, addAdminDto.Password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(newUser, addAdminDto.Role);

                if (!result.Succeeded)
                {
                    return ReturnErrors(result.Errors);
                }

                UserForDetailedDto userToReturn = _mapper.Map<UserForDetailedDto>(newUser);

                return LmsResponseHandler<UserForDetailedDto>.Successful(AddRoleToUser(userToReturn));
            }
            return ReturnErrors(result.Errors);
        }

        public async Task<IdentityResult> CreateUser(AppUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<AppUser> CompleteUserCreation(AppUser newUser, string newRole, string callbackUrl)
        {
            await _userManager.AddToRoleAsync(newUser, newRole);

            string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(newUser);

            await WelcomeMessage(resetPasswordToken, newUser, callbackUrl);

            return newUser;
        }

        public UserForDetailedDto AddRoleToUser(UserForDetailedDto user)
        {
            UserRoleDto role = user.UserRoles.ElementAtOrDefault(0);

            user.Role = role.Name;

            return user;
        }

        public IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users)
        {
            foreach (UserForDetailedDto user in users)
            {
                UserRoleDto role = user.UserRoles.ElementAtOrDefault(0);
                user.Role = role.Name;
            }

            return users;
        }

        public async Task<AppUser> GetAdminUser(int userId)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<IEnumerable<AppUser>> GetAdminUsers()
        {
            List<AppUser> users = await _userManager.Users.AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(EnumRoles.Member)))
                .OrderBy(u => u.FirstName).ToListAsync();

            return users;
        }

        public async Task UpdateUser(AppUser userforUpdate, string role)
        {
            bool isInRole = await _userManager.IsInRoleAsync(userforUpdate, role);

            if (!isInRole)
            {
                List<string> roles = new()
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

        public async Task DeleteUser(AppUser user)
        {
            _context.Remove(user);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"userID: {user.Id} was deleted");
        }

        private async Task WelcomeMessage(string code, AppUser user, string url)
        {
            string encodedToken = HttpUtility.UrlEncode(code);

            Uri callbackUrl = new Uri(url + user.Id + "/" + encodedToken);

            string body = $"Welcome {user.FirstName.ToLower()}, <p>An account has been created for you</p> Please create your new password by clicking <a href='{callbackUrl}'>here</a>:";

            await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        }

        private static LmsResponseHandler<UserForDetailedDto> ReturnErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<string> errors = new();

            foreach (IdentityError error in identityErrors)
            {
                errors.Add(error.Description);
            }

            return LmsResponseHandler<UserForDetailedDto>.Failed(errors);
        }
    }
}

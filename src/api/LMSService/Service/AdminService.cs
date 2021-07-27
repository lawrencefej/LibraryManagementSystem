using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Enumerations;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSRepository.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class AdminService : BaseService<AppUser, AdminUserForListDto, AdminUserForListDto, AdminService>, IAdminService
    {
        private readonly UserManager<AppUser> _userManager;
        // private readonly IEmailSender _emailSender;

        public AdminService(DataContext context, UserManager<AppUser> userManager,
            IEmailSender emailSender, ILogger<AdminService> logger, IMapper mapper) : base(context, mapper, logger)
        {
            _userManager = userManager;
            // _emailSender = emailSender;
        }

        public async Task<LmsResponseHandler<AdminUserForListDto>> CreateUser(AddAdminDto addAdminDto, bool password = false)
        {
            // TODO Implement email sent to user after creation
            AppUser newUser = Mapper.Map<AppUser>(addAdminDto);
            newUser.UserName = newUser.Email.ToLower();

            IdentityResult result = await CreateUser(addAdminDto, password, newUser);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(newUser, addAdminDto.Role);

                if (!result.Succeeded)
                {
                    return ReturnErrors(result.Errors);
                }

                AdminUserForListDto userToReturn = Mapper.Map<AdminUserForListDto>(newUser);

                return LmsResponseHandler<AdminUserForListDto>.Successful(userToReturn);
            }
            return ReturnErrors(result.Errors);
        }

        public async Task<LmsResponseHandler<AdminUserForListDto>> GetAdminUser(int userId)
        {
            AppUser user = await _userManager.Users.Include(m => m.UserRoles)
                .ThenInclude(s => s.Role)
                .Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(EnumRoles.Member)))
                .FirstOrDefaultAsync(u => u.Id == userId);

            return MapDetailReturn(user);
        }

        public async Task<PagedList<AdminUserForListDto>> GetAdminUsers(PaginationParams paginationParams)
        {
            IQueryable<AppUser> users = _userManager.Users.AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(EnumRoles.Member)))
                .OrderBy(u => u.FirstName).AsQueryable();

            return await FilterUsers(paginationParams, users);
        }

        public async Task<LmsResponseHandler<AdminUserForListDto>> UpdateUser(UpdateAdminRoleDto userforUpdate)
        {
            AppUser user = await _userManager.Users
                .Include(c => c.UserRoles)
                    .ThenInclude(ur => ur.Role).Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(MemberRole.Member))).FirstOrDefaultAsync(u => u.Id == userforUpdate.Id);

            if (user != null)
            {
                string oldRole = user.UserRoles.FirstOrDefault().Role.Name;

                if (oldRole != userforUpdate.Role.ToString())
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRole);

                    await _userManager.AddToRoleAsync(user, userforUpdate.Role.ToString());

                    return LmsResponseHandler<AdminUserForListDto>.Successful(Mapper.Map<AdminUserForListDto>(user));
                }

                return LmsResponseHandler<AdminUserForListDto>.Failed(new List<string>() { "User is already in the selected role" });

            }

            return LmsResponseHandler<AdminUserForListDto>.Failed(new List<string>() { "Selected user does not found" });
        }

        public async Task<LmsResponseHandler<AdminUserForListDto>> DeleteUser(int userId)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                Context.Remove(user);

                await Context.SaveChangesAsync();

                Logger.LogInformation($"userID: {user.Id} was deleted");

                return LmsResponseHandler<AdminUserForListDto>.Successful(Mapper.Map<AdminUserForListDto>(user));
            }

            return LmsResponseHandler<AdminUserForListDto>.Failed("Selected user does not found");
        }

        private async Task<PagedList<AdminUserForListDto>> FilterUsers(PaginationParams paginationParams, IQueryable<AppUser> users)
        {

            if (!string.IsNullOrEmpty(paginationParams.SearchString))
            {
                users = users
                    .Where(x => x.FirstName.Contains(paginationParams.SearchString)
                    || x.LastName.Contains(paginationParams.SearchString)
                    || x.Email.Contains(paginationParams.SearchString)
                    || x.PhoneNumber.Contains(paginationParams.SearchString)
                    );
            }

            users = paginationParams.SortDirection == "desc"
                ? string.Equals(paginationParams.OrderBy, "firstname", StringComparison.OrdinalIgnoreCase)
                    ? users.OrderByDescending(x => x.FirstName)
                    : string.Equals(paginationParams.OrderBy, "lastname", StringComparison.OrdinalIgnoreCase)
                        ? users.OrderByDescending(x => x.LastName)
                        : users.OrderByDescending(x => x.Email)
                : string.Equals(paginationParams.OrderBy, "firstname", StringComparison.OrdinalIgnoreCase)
                    ? users.OrderBy(x => x.FirstName)
                    : string.Equals(paginationParams.OrderBy, "lastname", StringComparison.OrdinalIgnoreCase)
                        ? users.OrderBy(x => x.LastName)
                        : users.OrderBy(x => x.Email);

            // PagedList<AppUser> usersToReturn = await PagedList<AppUser>.CreateAsync(users, paginationParams.PageNumber, paginationParams.PageSize);

            // return Mapper.Map<PagedList<AdminUserForList>>(usersToReturn);
            return await MapPagination(users, paginationParams);
        }

        // private async Task WelcomeMessage(string code, AppUser user, string url)
        // {
        //     string encodedToken = HttpUtility.UrlEncode(code);

        //     Uri callbackUrl = new Uri(url + user.Id + "/" + encodedToken);

        //     string body = $"Welcome {user.FirstName.ToLower()}, <p>An account has been created for you</p> Please create your new password by clicking <a href='{callbackUrl}'>here</a>:";

        //     await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        // }

        // private static AdminUserForList AddRoleToUser(AdminUserForList userForReturn, AppUser user)
        // {
        //     AppUserRole role = user.UserRoles.ElementAtOrDefault(0);

        //     userForReturn.Role = role.Role;

        //     return userForReturn;
        // }

        private async Task<IdentityResult> CreateUser(AddAdminDto addAdminDto, bool password, AppUser newUser)
        {
            return password ? await _userManager.CreateAsync(newUser, addAdminDto.Password) : await _userManager.CreateAsync(newUser);
        }

        // private IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users)
        // {
        //     foreach (UserForDetailedDto user in users)
        //     {
        //         UserRoleDto role = user.UserRoles.ElementAtOrDefault(0);
        //         user.Role = role.Name;
        //     }

        //     return users;
        // }

        // private async Task<AppUser> CompleteUserCreation(AppUser newUser, string newRole, string callbackUrl)
        // {
        //     await _userManager.AddToRoleAsync(newUser, newRole);

        //     string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(newUser);

        //     await WelcomeMessage(resetPasswordToken, newUser, callbackUrl);

        //     return newUser;
        // }
    }
}

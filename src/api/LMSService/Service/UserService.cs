using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSService.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class UserService : BaseService<AppUser, UserForDetailedDto, UserForDetailedDto, UserService>, IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        // private readonly IEmailService _emailSender;

        public UserService(IMapper mapper,
            DataContext context,
            UserManager<AppUser> userManager,
            // IEmailService emailSender,
            ILogger<UserService> logger,
            IHttpContextAccessor httpContextAccessor) : base(context, mapper, logger, httpContextAccessor)
        {
            _userManager = userManager;
            // _emailSender = emailSender;
        }

        public async Task<LmsResponseHandler<UserForDetailedDto>> GetUser(int userId)
        {
            if (!HttpContextAccessor.HttpContext.User.IsCurrentUser(userId))
            {
                return LmsResponseHandler<UserForDetailedDto>.Failed("");
            }

            AppUser user = await _userManager.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(user => user.Id == userId);

            return user != null
                ? LmsResponseHandler<UserForDetailedDto>.Successful(Mapper.Map<UserForDetailedDto>(user))
                : LmsResponseHandler<UserForDetailedDto>.Failed("");
        }

        public async Task<LmsResponseHandler<UserForDetailedDto>> ResetPassword(UserPasswordResetRequest userPasswordReset)
        {
            if (!HttpContextAccessor.HttpContext.User.IsCurrentUser(userPasswordReset.UserId))
            {
                return LmsResponseHandler<UserForDetailedDto>.Failed("");
            }

            AppUser user = await _userManager.Users.Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(user => user.Id == userPasswordReset.UserId);

            if (user == null)
            {
                return LmsResponseHandler<UserForDetailedDto>.Failed("");
            }

            bool signInResult = await _userManager.CheckPasswordAsync(user, userPasswordReset.CurrentPassword);

            if (!signInResult)
            {
                return LmsResponseHandler<UserForDetailedDto>.Failed("Invalid Password");
            }
            IdentityResult result = await _userManager.ChangePasswordAsync(user, userPasswordReset.CurrentPassword, userPasswordReset.NewPassword);

            if (result.Succeeded)
            {
                return LmsResponseHandler<UserForDetailedDto>.Successful();
            }
            else
            {
                Logger.LogInformation($"failed password change attempt", result.Errors);
                return LmsResponseHandler<UserForDetailedDto>.Failed("Something went wrong, please try again later");
            }
        }

        public async Task<LmsResponseHandler<UserForDetailedDto>> UpdateUserProfile(UserForUpdateDto userForUpdate)
        {
            if (!HttpContextAccessor.HttpContext.User.IsCurrentUser(userForUpdate.Id))
            {
                return LmsResponseHandler<UserForDetailedDto>.Failed("");
            }

            AppUser user = await _userManager.Users
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(user => user.Id == userForUpdate.Id);

            return user != null ? await UpdateUser(userForUpdate, user) : LmsResponseHandler<UserForDetailedDto>.Failed("");

        }

        private async Task<LmsResponseHandler<UserForDetailedDto>> UpdateUser(UserForUpdateDto userForUpdate, AppUser user)
        {
            Mapper.Map(userForUpdate, user);
            await _userManager.UpdateAsync(user);
            // await Context.SaveChangesAsync();
            return LmsResponseHandler<UserForDetailedDto>.Successful(Mapper.Map<UserForDetailedDto>(user));
        }

        // private async Task MemberWelcomeMessage(UserForDetailedDto user)
        // {
        //     string body = $"Welcome {TitleCase(user.FirstName)}, " +
        //         $"<p>A Sentinel Library account has been created for you.</p> " +
        //         $"<p>Your Library Card Number is {user.LibraryCardNumber}</p> " +
        //         $" " +
        //         $"<p>Thanks.</p> " +
        //         $"<p>Management</p>";

        //     await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        // }

        // public static string TitleCase(string strText)
        // {
        //     return new CultureInfo("en").TextInfo.ToTitleCase(strText.ToLower());
        // }

        // public async Task<IEnumerable<UserForDetailedDto>> SearchUsers(SearchUserDto searchUser)
        // {
        //     var users = await _context.SearchUsers(searchUser);

        //     var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

        //     return usersToReturn;
        // }

        // public async Task<PagedList<User>> GetAllMembersAsync(PaginationParams paginationParams)
        // {
        //     var users = _context.GetAll();

        //     return await PagedList<User>.CreateAsync(users, paginationParams.PageNumber, paginationParams.PageSize);
        // }
    }
}

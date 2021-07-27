using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Enumerations;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMSService.Service
{
    public class MemberService : IMemberService
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public MemberService(DataContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IdentityResult> CreateMember(AppUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        // public async Task<LmsResponseHandler<AppUser>> CreateMember(LibraryCard card)
        // {
        //     AppUser member = new()
        //     {
        //         FirstName = card.FirstName,
        //         LastName = card.LastName,
        //         Created = card.Created,
        //         Email = card.Email,
        //         UserName = card.Email,
        //         PhoneNumber = card.PhoneNumber
        //     };

        //     IdentityResult result = await _userManager.CreateAsync(member);


        //     if (result.Succeeded)
        //     {
        //         result = await _userManager.AddToRoleAsync(member, nameof(UserRoles.Member));

        //         if (!result.Succeeded)
        //         {
        //             return ReturnErrors(result.Errors);
        //         }

        //     }
        // }

        public async Task<AppUser> CompleteAddMember(AppUser member)
        {
            await _userManager.AddToRoleAsync(member, nameof(LmsAppRoles.Member));

            member.LibraryCard = await CreateNewCard(member.Id);

            await MemberWelcomeMessage(member);

            return member;
        }

        private async Task<LibraryCard> CreateNewCard(int memberId)
        {
            LibraryCard newCard = new()
            {
                MemberId = memberId
            };

            _context.Add(newCard);
            await _context.SaveChangesAsync();

            return newCard;
        }

        private async Task MemberWelcomeMessage(AppUser user)
        {
            var body = $"Welcome {TitleCase(user.FirstName)}, " +
                $"<p>A Sentinel Library account has been created for you.</p> " +
                $"<p>Your Library Card Number is {user.LibraryCard.Id}</p> " +
                $" " +
                $"<p>Thanks.</p> " +
                $"<p>Management</p>";

            await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        }

        public static string TitleCase(string strText)
        {
            return new CultureInfo("en").TextInfo.ToTitleCase(strText.ToLower());
        }

        public async Task DeleteMember(AppUser member)
        {
            // TODO Test
            _context.Remove(member);
            _context.Remove(member.LibraryCard);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<AppUser>> GetAllMembers(PaginationParams paginationParams)
        {
            IQueryable<AppUser> members = _userManager.Users.AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name == nameof(LmsAppRoles.Member)))
                .OrderBy(u => u.Email).AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.SearchString))
            {
                // TODO See if this can work with generics and then move to its own method
                members = members
                    .Where(x => x.FirstName.Contains(paginationParams.SearchString)
                    || x.LastName.Contains(paginationParams.SearchString)
                    || x.Email.Contains(paginationParams.SearchString)
                    );
            }

            if (paginationParams.SortDirection == "asc")
            {
                // TODO Same here
                if (string.Equals(paginationParams.OrderBy, "email", StringComparison.CurrentCultureIgnoreCase))
                {
                    members = members.OrderBy(x => x.Email);
                }
                else if (string.Equals(paginationParams.OrderBy, "firstname", StringComparison.CurrentCultureIgnoreCase))
                {
                    members = members.OrderBy(x => x.FirstName);
                }
                else if (string.Equals(paginationParams.OrderBy, "lastname", StringComparison.CurrentCultureIgnoreCase))
                {
                    members = members.OrderBy(x => x.Email);
                }
            }
            else if (paginationParams.SortDirection == "desc")
            {
                if (string.Equals(paginationParams.OrderBy, "email", StringComparison.CurrentCultureIgnoreCase))
                {
                    members = members.OrderByDescending(x => x.Email);
                }
                else if (string.Equals(paginationParams.OrderBy, "firstname", StringComparison.CurrentCultureIgnoreCase))
                {
                    members = members.OrderByDescending(x => x.FirstName);
                }
                else if (string.Equals(paginationParams.OrderBy, "lastname", StringComparison.CurrentCultureIgnoreCase))
                {
                    members = members.OrderByDescending(x => x.Email);
                }
            }
            else
            {
                members = members.OrderBy(x => x.Email);
            }

            return await PagedList<AppUser>.CreateAsync(members, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<AppUser>> AdvancedMemberSearch(UserForDetailedDto member)
        {
            IQueryable<AppUser> members = _userManager.Users.AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name == nameof(LmsAppRoles.Member)))
                .OrderBy(u => u.Email).AsQueryable();

            members = members
                .Where(x => x.FirstName.Contains(member.FirstName)
                || x.Email.Contains(member.LastName)
                || x.Email.Contains(member.Email)
                || x.PhoneNumber.Contains(member.PhoneNumber)
                );

            return await members.ToListAsync();
        }

        public async Task<AppUser> GetMember(int memberID)
        {
            AppUser user = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == memberID);

            return user;
        }

        public async Task<AppUser> GetMemberByCardNumber(int cardId)
        {
            AppUser user = await _userManager.Users
                .Include(c => c.LibraryCard)
                .Include(c => c.ProfilePicture)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.LibraryCard.Id == cardId);

            return user;
        }

        public async Task<IEnumerable<AppUser>> SearchMembers()
        {
            List<AppUser> users = await _userManager.Users.AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name == nameof(LmsAppRoles.Member)))
                .OrderBy(u => u.Email).ToListAsync();

            return users;
        }

        public Task<IEnumerable<AppUser>> SearchMembers(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMember(AppUser member)
        {
            _context.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesMemberExist(string email)
        {
            AppUser user = await _context.Users.AsNoTracking()
                .Where(u => u.UserRoles.Any(r => r.Role.Name == nameof(LmsAppRoles.Member)))
                .FirstOrDefaultAsync(x => x.Email == email);
            return user != null;
        }
    }
}

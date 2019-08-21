using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class MemberService : IMemberService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public MemberService(DataContext context, UserManager<User> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<User> AddMember(User member)
        {
            member.UserName = member.Email;

            var result = await _userManager.CreateAsync(member);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(member, nameof(EnumRoles.Member));

                member.LibraryCard = await CreateNewCard(member.Id);

                await MemberWelcomeMessage(member);

                return member;
            }

            throw new Exception($"Adding member failed on save");
        }

        private async Task<LibraryCard> CreateNewCard(int memberId)
        {
            var newCard = new LibraryCard
            {
                UserId = memberId
            };

            _context.Add(newCard);
            await _context.SaveChangesAsync();

            return newCard;
        }

        private async Task MemberWelcomeMessage(User user)
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

        public async Task DeleteMember(User user)
        {
            // TODO Test
            _context.Remove(user);
            _context.Remove(user.LibraryCard);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<User>> GetAllMembers(PaginationParams paginationParams)
        {
            var users = _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name == nameof(EnumRoles.Member)))
                .OrderBy(u => u.Lastname).AsQueryable();

            return await PagedList<User>.CreateAsync(users, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<User> GetMembers(int memberID)
        {
            var user = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == memberID);

            return user;
        }

        public async Task<User> GetMemberByCardNumber(int cardId)
        {
            var user = await _userManager.Users
                .Include(c => c.LibraryCard)
                .Include(c => c.ProfilePicture)
                .Include(c => c.UserRoles)
                .FirstOrDefaultAsync(u => u.LibraryCard.Id == cardId);

            return user;
        }

        public async Task<IEnumerable<User>> SearchMembers()
        {
            var users = await _userManager.Users
                .Include(p => p.ProfilePicture)
                .Include(c => c.LibraryCard)
                .Include(c => c.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role.Name == nameof(EnumRoles.Member)))
                .OrderBy(u => u.Lastname).ToListAsync();

            return users;
        }

        public Task<IEnumerable<User>> SearchMembers(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMember(User member)
        {
            _context.Update(member);
            await _context.SaveChangesAsync();
        }
    }
}
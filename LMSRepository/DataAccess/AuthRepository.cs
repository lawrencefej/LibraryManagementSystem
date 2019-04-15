using LMSRepository.Data;
using LMSRepository.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces.DataAccess
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AuthRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> Login(string emailAddress, string password)
        {
            var user = await _context.Users.Include(p => p.ProfilePicture).FirstOrDefaultAsync(x => x.Email == emailAddress);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public Task<User> Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ResetPassword(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public Task<bool> UserExists(string username)
        {
            throw new NotImplementedException();
        }
    }
}
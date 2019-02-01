using LMSLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
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

        public Task<bool> UserExists(string username)
        {
            throw new NotImplementedException();
        }
    }
}
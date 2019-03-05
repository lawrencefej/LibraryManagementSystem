using LMSRepository.Data;
using LMSRepository.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces.DataAccess
{
    public class LibraryCardRepository : ILibraryCardRepository
    {
        private readonly DataContext _context;

        public LibraryCardRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<LibraryCard>> GetAllLibraryCards()
        {
            var idCards = await _context.LibraryCards.OrderBy(o => o.Created)
                .ToListAsync();

            return idCards;
        }

        public async Task<LibraryCard> GetCard(int id)
        {
            var cardId = await _context.LibraryCards
                .FirstOrDefaultAsync(p => p.Id == id);

            return cardId;
        }

        public async Task<LibraryCard> GetMemberCard(int userId)
        {
            var libraryCard = await _context.LibraryCards
                .Include(c => c.Checkouts)
                .Include(c => c.ReservedAssets)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return libraryCard;
        }
    }
}
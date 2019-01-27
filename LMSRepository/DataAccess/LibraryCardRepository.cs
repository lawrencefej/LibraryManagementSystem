using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSLibrary.Data
{
    public class LibraryCardRepository : ILibraryCardRepository
    {
        private const string reserved = "Reserved";
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

        public async Task<LibraryCard> GetMemberCard(int id)
        {
            var libraryCard = await _context.LibraryCards
                //.Include(u => u.ReservedAssets)
                .FirstOrDefaultAsync(p => p.UserId == id);

            return libraryCard;
        }
    }
}

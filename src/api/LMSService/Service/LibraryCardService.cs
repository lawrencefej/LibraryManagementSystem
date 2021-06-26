using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.Enumerations;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class LibraryCardService : ILibraryCardService
    {
        private readonly DataContext _context;
        private readonly ILogger<LibraryCard> _logger;
        private readonly UserManager<AppUser> _userManager;
        public LibraryCardService(DataContext context, ILogger<LibraryCard> logger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;

        }
        public async Task<LibraryCard> AddLibraryCard(LibraryCard card)
        {
            card.CardNumber = card.GenerateCardNumber();
            AppUser member = await CreateNewMember(card);
            card.Member = member;

            _context.Add(card);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"added LibraryCard {card.CardNumber} with ID: {card.Id}");

            return card;
        }

        private async Task<AppUser> CreateNewMember(LibraryCard card)
        {
            AppUser member = new()
            {
                FirstName = card.FirstName,
                LastName = card.LastName,
                Created = card.Created,
                Email = card.Email,
                UserName = card.Email
            };

            await _userManager.CreateAsync(member);
            await _userManager.AddToRoleAsync(member, nameof(UserRoles.Member));

            return member;
        }

        public async Task DeleteLibraryCard(LibraryCard card)
        {

            _context.Remove(card);
            await _context.SaveChangesAsync();
            // TODO log who performed the action

            _logger.LogInformation($"LibraryCard with ID {card.Id} was deleted");
            return;
        }

        public async Task<bool> DoesLibraryCardExist(string cardNumber)
        {
            // LibraryCard card = await _context.LibraryCards.AsNoTracking().FirstOrDefaultAsync(x => x.CardNumber == cardNumber);

            // return card != null;

            return await _context.LibraryCards.AsNoTracking().FirstOrDefaultAsync(x => x.CardNumber == cardNumber) == null;
        }

        public async Task<PagedList<LibraryCard>> GetAllLibraryCard(PaginationParams paginationParams)
        {
            IQueryable<LibraryCard> cards = _context.LibraryCards.AsNoTracking()
                .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State)
                .OrderBy(u => u.CardNumber).AsQueryable();

            return await PagedList<LibraryCard>.CreateAsync(cards, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<LibraryCard> GetLibraryCardByNumber(string cardNumber)
        {
            return await _context.LibraryCards.AsNoTracking()
                .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State).FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }

        public async Task<LibraryCard> GetLibraryCardById(int id)
        {
            return await _context.LibraryCards.AsNoTracking()
                .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<LibraryCard>> SearchLibraryCard(LibraryCard card)
        {
            IQueryable<LibraryCard> cards = _context.LibraryCards.AsNoTracking()
                .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State)
                .OrderBy(u => u.CardNumber).AsQueryable();

            cards = cards
                .Where(x => x.FirstName.Contains(card.FirstName)
                || x.Email.Contains(card.LastName)
                || x.Email.Contains(card.Email)
                || x.PhoneNumber.Contains(card.PhoneNumber)
                );

            return await cards.ToListAsync();

        }

        public async Task<LibraryCard> UpdateLibraryCard(LibraryCard card)
        {
            _context.Update(card);
            await _context.SaveChangesAsync();

            return card;
        }
    }
}

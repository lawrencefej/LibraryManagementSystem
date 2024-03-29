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
using LMSService.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class LibraryCardService : BaseService<LibraryCard, LibraryCardForDetailedDto, LibrarycardForListDto, LibraryCardService>, ILibraryCardService
    {
        private readonly UserManager<AppUser> _userManager;
        public LibraryCardService(DataContext context, ILogger<LibraryCardService> logger, UserManager<AppUser> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(context, mapper, logger, httpContextAccessor)
        {
            _userManager = userManager;
        }

        public async Task<LmsResponseHandler<LibraryCardForDetailedDto>> AddLibraryCard(LibraryCardForCreationDto addCardDto)
        {
            if (Context.LibraryCards.Any(a => a.Email == addCardDto.Email))
            {
                return LmsResponseHandler<LibraryCardForDetailedDto>.Failed($"{addCardDto.Email} already exists");
            }

            LibraryCard card = Mapper.Map<LibraryCard>(addCardDto);

            // LmsResponseHandler<AppUser> member = await CreateNewMember(card);

            // if (member.Succeeded)
            // {
            card = await GenerateCardNumber(card);
            // card.Member = member.Item;

            Context.Add(card);
            await Context.SaveChangesAsync();

            Logger.LogInformation($"added LibraryCard {card.CardNumber} with ID: {card.Id}");

            LibraryCardForDetailedDto cardsToReturn = Mapper.Map<LibraryCardForDetailedDto>(card);

            return LmsResponseHandler<LibraryCardForDetailedDto>.Successful(cardsToReturn);
            // }

            // return LmsResponseHandler<LibraryCardForDetailedDto>.Failed(member.Errors);
        }

        public async Task<LmsResponseHandler<LibraryCardForDetailedDto>> DeleteLibraryCard(int cardId)
        {
            LibraryCard card = await GetLibraryCard(cardId);

            if (card != null)
            {

                card.Status = LibraryCardStatus.Deactivated;
                Context.Update(card);
                await Context.SaveChangesAsync();
                // TODO log who performed the action

                Logger.LogInformation($"LibraryCard with ID {card} was deleted");
                return LmsResponseHandler<LibraryCardForDetailedDto>.Successful();
            }

            return LmsResponseHandler<LibraryCardForDetailedDto>.Failed($"Selected Card does not exist");
        }

        public async Task<LmsResponseHandler<LibraryCardForDetailedDto>> ReactivateLibraryCard(int cardId)
        {
            LibraryCard card = await GetLibraryCard(cardId);

            if (card != null)
            {

                card.Status = LibraryCardStatus.Good;
                Context.Update(card);
                await Context.SaveChangesAsync();
                // TODO log who performed the action

                Logger.LogInformation($"LibraryCard with ID {card} was activated");
                return LmsResponseHandler<LibraryCardForDetailedDto>.Successful();
            }

            return LmsResponseHandler<LibraryCardForDetailedDto>.Failed($"Selected Card does not exist");
        }

        public async Task<PagedList<LibrarycardForListDto>> GetAllLibraryCard(PaginationParams paginationParams)
        {
            IQueryable<LibraryCard> cards = Context.LibraryCards.AsNoTracking()
                .Where(m => m.Status != LibraryCardStatus.Deactivated)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                .ThenInclude(s => s.State)
                .OrderBy(u => u.CardNumber)
                .AsQueryable();

            return await FilterCards(paginationParams, cards);
        }

        public async Task<LmsResponseHandler<LibraryCardForDetailedDto>> GetLibraryCardByNumber(string cardNumber)
        {
            LibraryCard card = await Context.LibraryCards.AsNoTracking()
                .Where(m => m.Status != LibraryCardStatus.Deactivated)
                // .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State).FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

            return MapDetailReturn(card);
        }

        public async Task<LmsResponseHandler<LibraryCardForDetailedDto>> GetLibraryCardById(int id)
        {
            LibraryCard card = await Context.LibraryCards.AsNoTracking()
                .Where(m => m.Status != LibraryCardStatus.Deactivated)
                .Include(m => m.Checkouts.Where(s => s.Status == CheckoutStatus.Checkedout)).ThenInclude(b => b.LibraryAsset)
                .Include(m => m.Checkouts)
                // .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                .ThenInclude(s => s.State)
                .FirstOrDefaultAsync(c => c.Id == id);

            return MapDetailReturn(card);
        }

        private async Task<LibraryCard> GetLibraryCard(int id)
        {
            return await Context.LibraryCards.AsNoTracking()
                .Where(m => m.Status != LibraryCardStatus.Deactivated)
                // .Include(m => m.Member)
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                .ThenInclude(s => s.State)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // TODO fix filter
        public async Task<PagedList<LibrarycardForListDto>> AdvancedLibraryCardSearch(LibraryCardForAdvancedSearch card, PaginationParams paginationParams)
        {
            IQueryable<LibraryCard> cards = Context.LibraryCards.AsNoTracking()
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State)
                .OrderBy(u => u.CardNumber).AsQueryable();

            cards = cards
                .Where(x => x.FirstName == card.FirstName
                && x.Email == card.LastName
                && x.Email == card.Email
                && x.PhoneNumber == card.PhoneNumber
                && x.Address.Zipcode == card.Zipcode
                && x.DateOfBirth == card.DateOfBirth
                );

            paginationParams.SearchString = null;

            return await FilterCards(paginationParams, cards);
        }

        public async Task<IEnumerable<LibraryCardForDetailedDto>> AdvancedLibraryCardSearch(LibraryCardForAdvancedSearch card)
        {
            IQueryable<LibraryCard> cards = Context.LibraryCards.AsNoTracking()
                .Include(m => m.LibraryCardPhoto)
                .Include(m => m.Address)
                    .ThenInclude(s => s.State)
                .OrderBy(u => u.CardNumber).AsQueryable();

            List<LibraryCard> assetToReturn = await cards
                .Where(x => x.FirstName.Contains(card.FirstName)
                || x.Email.Contains(card.LastName)
                || x.Email.Contains(card.Email)
                || x.PhoneNumber == card.PhoneNumber
                || x.Address.Zipcode.Contains(card.Zipcode)
                || x.DateOfBirth == card.DateOfBirth
                ).ToListAsync();

            return Mapper.Map<IEnumerable<LibraryCardForDetailedDto>>(assetToReturn);
        }

        public async Task<LmsResponseHandler<LibraryCardForDetailedDto>> UpdateLibraryCard(LibraryCardForUpdate cardForUpdate)
        {
            LibraryCard card = await GetLibraryCard(cardForUpdate.Id);

            if (card != null)
            {
                Mapper.Map(cardForUpdate, card);
                Context.Update(card);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"LibraryCard: {card.Id} was edited");

                return LmsResponseHandler<LibraryCardForDetailedDto>.Successful(Mapper.Map<LibraryCardForDetailedDto>(card));
            }

            return LmsResponseHandler<LibraryCardForDetailedDto>.Failed($"LibraryCard: {card.CardNumber} does not exist");
        }

        private async Task<LibraryCard> GenerateCardNumber(LibraryCard card)
        {
            card.GenerateCardNumber();

            if (await DoesLibraryCardExist(card.CardNumber))
            {
                card.GenerateCardNumber();

                await GenerateCardNumber(card);
            }

            return card;
        }

        private async Task<LmsResponseHandler<AppUser>> CreateNewMember(LibraryCard card)
        {
            AppUser member = new()
            {
                FirstName = card.FirstName,
                LastName = card.LastName,
                Created = card.Created,
                Email = card.Email,
                UserName = card.Email,
                PhoneNumber = card.PhoneNumber,
                Gender = card.Gender
            };

            IdentityResult result = await _userManager.CreateAsync(member);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(member, nameof(LmsAppRoles.Member));

                if (!result.Succeeded)
                {
                    return ErrorMapper<AppUser>.ReturnErrors(result.Errors);
                }

            }

            return LmsResponseHandler<AppUser>.Successful(member);
        }

        private async Task<PagedList<LibrarycardForListDto>> FilterCards(PaginationParams paginationParams, IQueryable<LibraryCard> cards)
        {

            if (!string.IsNullOrEmpty(paginationParams.SearchString))
            {
                cards = cards
                    .Where(x => x.FirstName.Contains(paginationParams.SearchString)
                    || x.LastName.Contains(paginationParams.SearchString)
                    || x.Email.Contains(paginationParams.SearchString)
                    || x.PhoneNumber.Contains(paginationParams.SearchString)
                    || x.CardNumber.Contains(paginationParams.SearchString)
                    || x.Address.Street.Contains(paginationParams.SearchString)
                    );
            }

            cards = paginationParams.SortDirection == "desc"
                ? string.Equals(paginationParams.OrderBy, "firstname", StringComparison.OrdinalIgnoreCase)
                    ? cards.OrderByDescending(x => x.FirstName)
                    : string.Equals(paginationParams.OrderBy, "lastname", StringComparison.OrdinalIgnoreCase)
                        ? cards.OrderByDescending(x => x.LastName)
                        : cards.OrderByDescending(x => x.Email)
                : string.Equals(paginationParams.OrderBy, "firstname", StringComparison.OrdinalIgnoreCase)
                    ? cards.OrderBy(x => x.FirstName)
                    : string.Equals(paginationParams.OrderBy, "lastname", StringComparison.OrdinalIgnoreCase)
                        ? cards.OrderBy(x => x.LastName)
                        : cards.OrderBy(x => x.Email);

            return await MapPagination(cards, paginationParams);
        }

        private async Task<bool> DoesLibraryCardExist(string cardNumber)
        {
            return await Context.LibraryCards.AsNoTracking().AnyAsync(x => x.CardNumber == cardNumber);
        }
    }
}

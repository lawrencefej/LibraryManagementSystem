using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Identity;

namespace LMSContracts.Interfaces
{
    public interface IMemberService
    {
        Task<AppUser> GetMember(int memberID);

        Task<AppUser> GetMemberByCardNumber(int cardId);

        Task<bool> DoesMemberExist(string email);

        Task<IEnumerable<AppUser>> SearchMembers();

        Task<IEnumerable<AppUser>> SearchMembers(string searchString);

        Task<IEnumerable<AppUser>> AdvancedMemberSearch(UserForDetailedDto member);

        Task UpdateMember(AppUser member);

        Task<IdentityResult> CreateMember(AppUser user);

        Task<AppUser> CompleteAddMember(AppUser member);

        Task DeleteMember(AppUser member);

        Task<PagedList<AppUser>> GetAllMembers(PaginationParams paginationParams);
    }
}

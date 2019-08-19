using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IMemberService
    {
        Task<User> GetMembers(int memberID);

        Task<User> GetMemberByCardNumber(int cardId);

        Task<IEnumerable<User>> SearchMembers();

        Task<IEnumerable<User>> SearchMembers(string searchString);

        Task UpdateMember(User member);

        Task<User> AddMember(User member);

        Task DeleteMember(User member);

        Task<PagedList<User>> GetAllMembers(PaginationParams paginationParams);
    }
}

using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Dto
{
    public interface IUserService
    {
        Task<IEnumerable<UserForListDto>> GetUsers();

        Task<UserForDetailedDto> GetUser(int userId);

        Task<UserForDetailedDto> GetUserByEmail(string email);

        Task<UserForDetailedDto> GetUserByCardNumber(int cardId);

        Task<IEnumerable<UserForDetailedDto>> SearchUsers(SearchUserDto searchUser);

        Task<IEnumerable<UserForListDto>> SearchUsers(string searchString);

        Task UpdateMember(UserForUpdateDto userForUpdate);

        Task<UserForDetailedDto> AddMember(MemberForCreation memberForCreation);

        Task DeleteUser(int userId);

        Task<PagedList<User>> GetAllMembersAsync(PaginationParams paginationParams);
    }
}
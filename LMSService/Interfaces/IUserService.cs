using LMSRepository.Dto;
using LMSRepository.Interfaces.Dto;
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

        Task<UserForDetailedDto> SearchUser(SearchUserDto searchUser);

        Task<IEnumerable<UserForListDto>> SearchUsers(string searchString);

        Task UpdateUser(UserForUpdateDto userForUpdate);

        Task<UserForDetailedDto> AddMember(MemberForCreation memberForCreation);

        Task DeleteUser(int userId);
    }
}
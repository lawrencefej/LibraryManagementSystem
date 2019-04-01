using LMSRepository.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Dto
{
    public interface IAdminService
    {
        Task<IEnumerable<UserForDetailedDto>> GetAdminUsers();

        Task<UserForDetailedDto> CreateUser(AddAdminDto addAdminDto);

        Task<UserForDetailedDto> GetAdminUser(int userId);
    }
}
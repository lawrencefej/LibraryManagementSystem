using LMSRepository.Dto;
using LMSRepository.Interfaces.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Dto
{
    public interface IAdminService
    {
        Task<IEnumerable<UserForDetailedDto>> GetAdminUsers();

        Task<UserForDetailedDto> CreateAdminUser(AddAdminDto addAdminDto);

        Task<UserForDetailedDto> GetAdminUser(int userId);
    }
}
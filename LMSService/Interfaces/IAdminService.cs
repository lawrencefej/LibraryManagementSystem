using LMSRepository.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfacees
{
    public interface IAdminService
    {
        Task<IEnumerable<UserForDetailedDto>> GetAdminUsers();

        Task<UserForDetailedDto> CreateUser(AddAdminDto addAdminDto);

        Task UpdateUser(UpdateAdminDto userforUpdate);

        Task DeleteUser(int userId);

        Task<UserForDetailedDto> GetAdminUser(int userId);
    }
}
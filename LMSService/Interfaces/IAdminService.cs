using LMSRepository.Interfaces.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserForDetailedDto>> GetAdminUsers();

        Task<UserForDetailedDto> CreateAdminUser();

        Task<UserForDetailedDto> GetAdminUser(int userId);
    }
}
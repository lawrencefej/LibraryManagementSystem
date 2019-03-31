using AutoMapper;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Dto;
using LMSService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LMSRepository.Dto;

namespace LMSService.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        public Task<UserForDetailedDto> CreateAdminUser()
        {
            throw new NotImplementedException();
        }

        public Task<UserForDetailedDto> GetAdminUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserForDetailedDto>> GetAdminUsers()
        {
            var users = await _adminRepository.GetAdminUsers();

            //var role = new RoleDto();

            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

            foreach (var user in usersToReturn)
            {
                var role = user.UserRoles.ElementAtOrDefault(0);
                user.Role = role.Name;
            }

            return usersToReturn;
        }
    }
}
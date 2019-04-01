using AutoMapper;
using LMSRepository.Interfaces;
using LMSService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LMSRepository.Dto;
using LMSRepository.Interfaces.Models;

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

        public async Task<UserForDetailedDto> CreateUser(AddAdminDto addAdminDto)
        {
            addAdminDto.UserName = addAdminDto.Email.ToLower();

            addAdminDto.Password = CreatePassword(addAdminDto.FirstName, addAdminDto.LastName);

            var userToCreate = _mapper.Map<User>(addAdminDto);

            await _adminRepository.CreateUser(userToCreate, addAdminDto.Password, addAdminDto.Role);

            //addAdminDto.Id = userToCreate.Id;

            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

            var role = userToReturn.UserRoles.ElementAt(0);

            userToReturn.Role = role.Name;

            return userToReturn;
        }

        public Task<UserForDetailedDto> GetAdminUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserForDetailedDto>> GetAdminUsers()
        {
            var users = await _adminRepository.GetAdminUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

            foreach (var user in usersToReturn)
            {
                var role = user.UserRoles.ElementAtOrDefault(0);
                user.Role = role.Name;
            }

            return usersToReturn;
        }

        private static string CreatePassword(string fname, string lname)
        {
            var test = fname.Substring(0, 1).ToUpper();

            var second = lname.ToLower();

            var password = string.Concat(test, second, DateTime.Today.Year.ToString());

            return password;
        }
    }
}
using AutoMapper;
using LMSRepository.Interfaces;
using LMSService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LMSRepository.Dto;
using LMSRepository.Interfaces.Models;
using LMSService.Exceptions;

namespace LMSService.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _libraryRepository;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, ILibraryRepository libraryRepository)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _libraryRepository = libraryRepository;
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

        public async Task<UserForDetailedDto> GetAdminUser(int userId)
        {
            var user = await _adminRepository.GetAdminUser(userId);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task<IEnumerable<UserForDetailedDto>> GetAdminUsers()
        {
            var users = await _adminRepository.GetUsers();

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
            var firstInitial = fname.Substring(0, 1).ToUpper();

            var lastName = lname.ToLower();

            var password = string.Concat(firstInitial, lastName, DateTime.Today.Year.ToString());

            return password;
        }

        public async Task UpdateUser(UpdateAdminDto userforUpdate)
        {
            //var user = await _adminRepository.GetAdminUser(addAdminDto.Id);

            //_mapper.Map(addAdminDto, user);

            var user = await _adminRepository.GetAdminUser(userforUpdate.Id);

            _mapper.Map(userforUpdate, user);

            await _adminRepository.UpdateUser(user, userforUpdate.Role);

            if (await _libraryRepository.SaveAll())
            {
                return;
            }

            throw new Exception($"Updating user failed on save");
        }
    }
}
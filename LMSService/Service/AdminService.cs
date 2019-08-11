using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Interfacees;
using LMSService.Exceptions;
using LMSService.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LMSService.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, ILibraryRepository libraryRepository,
            IEmailSender emailSender, IAuthRepository authRepository, ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _libraryRepository = libraryRepository;
            _emailSender = emailSender;
            _authRepository = authRepository;
            _logger = logger;
        }

        public async Task<UserForDetailedDto> CreateUser(AddAdminDto addAdminDto)
        {
            addAdminDto.UserName = addAdminDto.Email;

            var userToCreate = _mapper.Map<User>(addAdminDto);

            await _adminRepository.CreateUser(userToCreate, addAdminDto.Role);

            var resetPasswordToken = await _authRepository.ResetPassword(userToCreate);

            await WelcomeMessage(resetPasswordToken, userToCreate, addAdminDto.CallbackUrl);

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
                //Add the role to the user
                var role = user.UserRoles.ElementAtOrDefault(0);
                user.Role = role.Name;
            }

            return usersToReturn;
        }

        public async Task UpdateUser(UpdateAdminDto userforUpdate)
        {
            var user = await _adminRepository.GetAdminUser(userforUpdate.Id);

            _mapper.Map(userforUpdate, user);

            await _adminRepository.UpdateUser(user, userforUpdate.Role);

            if (await _libraryRepository.SaveAll())
            {
                return;
            }

            throw new Exception($"Updating user failed on save");
        }

        private async Task WelcomeMessage(string code, User user, string url)
        {
            var encodedToken = HttpUtility.UrlEncode(code);

            var callbackUrl = new Uri(url + user.Id + "/" + encodedToken);

            var body = $"Welcome {user.FirstName.ToLower()}, <p>An account has been created for you</p> Please create your new password by clicking <a href='{callbackUrl}'>here</a>:";

            await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _adminRepository.GetAdminUser(userId);

            if (user == null)
            {
                _logger.LogWarning($"userID: {userId} was not found");
                throw new NoValuesFoundException($"userID: {userId} was not found");
            }

            _libraryRepository.Delete(user);

            if (await _libraryRepository.SaveAll())
            {
                _logger.LogInformation($"userID: {user.Id} was deleted");
                return;
            }

            _logger.LogCritical($"Deleting userID: {user.Id} failed on save");
            throw new Exception($"Deleting userID: {user.Id} failed on save");
        }
    }
}
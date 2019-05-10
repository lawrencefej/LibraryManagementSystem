using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Dto;
using LMSService.Helpers;
using LMSService.Interfaces;
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

        public AdminService(IAdminRepository adminRepository, IMapper mapper, ILibraryRepository libraryRepository,
            IEmailSender emailSender, IAuthRepository authRepository)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _libraryRepository = libraryRepository;
            _emailSender = emailSender;
            _authRepository = authRepository;
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

        //public async Task<UserForDetailedDto> CreateUser2(AddAdminDto addAdminDto)
        //{
        //    addAdminDto.UserName = addAdminDto.Email;

        //    addAdminDto.Password = CreatePassword(addAdminDto.FirstName, addAdminDto.LastName);

        //    var userToCreate = _mapper.Map<User>(addAdminDto);

        //    await _adminRepository.CreateUser(userToCreate, addAdminDto.Password, addAdminDto.Role);

        //    var resetPasswordToken = await _authRepository.ResetPassword(userToCreate);

        //    await WelcomeMessage(resetPasswordToken, userToCreate, addAdminDto.CallbackUrl);

        //    var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

        //    var role = userToReturn.UserRoles.ElementAt(0);

        //    userToReturn.Role = role.Name;

        //    return userToReturn;
        //}

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

            var token = LmsTokens.GenerateJwtToken(user, encodedToken);

            var callbackUrl = new Uri(url + "/" + token);

            var body = $"Welcome {user.FirstName.ToLower()}, <p>An account has been created for you</p> Please create your new password by clicking here: <a href='{callbackUrl}'>link</a>";

            await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
        }
    }
}
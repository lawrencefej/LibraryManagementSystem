using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Dto;
using LMSService.Exceptions;
using LMSService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserForDetailedDto> GetUser(int userId)
        {
            var user = await _userRepo.GetUser(userId);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task<UserForDetailedDto> SearchUser(SearchUserDto searchUser)
        {
            var user = await _userRepo.SearchUser(searchUser);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task<UserForDetailedDto> GetUserByCardNumber(int cardId)
        {
            var user = await _userRepo.GetUserByCardId(cardId);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task<UserForDetailedDto> GetUserByEmail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task<IEnumerable<UserForListDto>> GetUsers()
        {
            var users = await _userRepo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return usersToReturn;
        }

        public async Task<IEnumerable<UserForListDto>> SearchUsers(string searchString)
        {
            var users = await _userRepo.SearchUsers(searchString);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return usersToReturn;
        }

        public async Task UpdateUser(UserForUpdateDto userForUpdate)
        {
            var user = await _userRepo.GetUser(userForUpdate.Id);

            if (user == null)
            {
                throw new NoValuesFoundException("User was not found");
            }

            _mapper.Map(userForUpdate, user);

            if (await _userRepo.SaveAll())
            {
                return;
            }

            throw new Exception($"Updating user failed on save");
        }
    }
}
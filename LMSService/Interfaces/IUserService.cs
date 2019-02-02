﻿using LMSLibrary.Dto;
using LMSRepository.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserForListDto>> GetUsers();

        Task<UserForDetailedDto> GetUser(int userId);

        Task<UserForDetailedDto> GetUserByEmail(string email);

        Task<UserForDetailedDto> GetUserByCardNumber(int cardId);

        Task<UserForDetailedDto> SearchUser(SearchUserDto searchUser);
    }
}
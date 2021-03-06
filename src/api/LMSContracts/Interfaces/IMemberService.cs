﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Identity;

namespace LMSContracts.Interfaces
{
    public interface IMemberService
    {
        Task<User> GetMember(int memberID);

        Task<User> GetMemberByCardNumber(int cardId);

        Task<bool> DoesMemberExist(string email);

        Task<IEnumerable<User>> SearchMembers();

        Task<IEnumerable<User>> SearchMembers(string searchString);

        Task<IEnumerable<User>> AdvancedMemberSearch(UserForDetailedDto member);

        Task UpdateMember(User member);

        Task<IdentityResult> CreateMember(User user);

        Task<User> CompleteAddMember(User member);

        Task DeleteMember(User member);

        Task<PagedList<User>> GetAllMembers(PaginationParams paginationParams);
    }
}

using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;

namespace LMSContracts.Interfaces
{
    public interface IAdminService
    {
        Task<LmsResponseHandler<AdminUserForListDto>> CreateUser(AddAdminDto addAdminDto, bool password = false);


        Task<LmsResponseHandler<AdminUserForListDto>> GetAdminUser(int userId);


        Task<LmsResponseHandler<AdminUserForListDto>> UpdateUser(UpdateAdminRoleDto userforUpdate);


        Task<LmsResponseHandler<AdminUserForListDto>> DeleteUser(int userId);


        Task<PagedList<AdminUserForListDto>> GetAdminUsers(PaginationParams paginationParams);
    }
}

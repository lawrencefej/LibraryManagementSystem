using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;

namespace LMSContracts.Interfaces
{
    public interface IUserService
    {
        Task<LmsResponseHandler<UserForDetailedDto>> GetUser(int userId);

        Task<LmsResponseHandler<UserForDetailedDto>> ResetPassword(UserPasswordResetRequest userPasswordReset);

        Task<LmsResponseHandler<UserForDetailedDto>> UpdateUserProfile(UserForUpdateDto userForUpdate);
    }
}

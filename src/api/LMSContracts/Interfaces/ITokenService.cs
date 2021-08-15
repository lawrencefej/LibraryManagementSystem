using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GetLoginToken(AppUser user, string ipAddress);
        Task RevokeToken(string token);

        Task<LmsResponseHandler<TokenResponseDto>> RefreshToken(TokenRequestDto tokenRequestDto, string ipAddress);
    }
}

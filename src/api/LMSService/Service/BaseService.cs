using LMSContracts.Interfaces;
using LMSService.Extensions;
using Microsoft.AspNetCore.Http;

namespace LMSService.Service
{
    public class BaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        protected int GetLoggedInUserId()
        {
            return _httpContextAccessor.HttpContext.User.GetUserId();
        }

        protected int GetLoggedInUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.GetUserId();
        }

        protected bool IsCurrentUser(int id)
        {
            return _httpContextAccessor.HttpContext.User.IsCurrentUser(id);
        }

    }
}

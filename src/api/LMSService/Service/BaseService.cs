using System.Collections.Generic;
using LMSEntities.Helpers;
using LMSService.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMSService.Service
{
    public class BaseService<TDetail>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        protected static LmsResponseHandler<TDetail> ReturnErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<string> errors = new();

            foreach (IdentityError error in identityErrors)
            {
                errors.Add(error.Description);
            }

            return LmsResponseHandler<TDetail>.Failed(errors);
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

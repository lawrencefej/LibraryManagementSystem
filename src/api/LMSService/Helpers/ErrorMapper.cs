using System.Collections.Generic;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Identity;

namespace LMSService.Helpers
{
    public static class ErrorMapper<T>
    {
        public static LmsResponseHandler<T> ReturnErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<string> errors = new();

            foreach (IdentityError error in identityErrors)
            {
                errors.Add(error.Description);
            }

            return LmsResponseHandler<T>.Failed(errors);
        }
    }
}

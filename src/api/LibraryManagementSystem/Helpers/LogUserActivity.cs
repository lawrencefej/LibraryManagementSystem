using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // TODO Figure out if this is still needed
            //var resultContext = await next();

            //var userId = int.Parse(resultContext.HttpContext.User
            //.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            //var user = await repo.GetUser(userId);
            ////user.LastActive = DateTime.Now;
            //await repo.SaveAll();
        }
    }
}
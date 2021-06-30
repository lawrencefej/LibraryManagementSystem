using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace LibraryManagementSystem.Helpers
{
    public class DevOnlyActionFilter : IActionFilter
    {
        public IWebHostEnvironment Env { get; }
        public DevOnlyActionFilter(IWebHostEnvironment env)
        {
            Env = env;

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // TODO Test This
            if (!Env.IsDevelopment())
            {
                context.Result = new NotFoundResult();
                return;
            }
        }
    }
}

using LMSEntities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController<T> : ControllerBase
    {
        protected virtual IActionResult ResultCheck(LmsResponseHandler<T> result)
        {
            return result.Succeeded ? result.Item != null ? Ok(result.Item) : NoContent() : NotFound(result.Error);
        }

    }
}

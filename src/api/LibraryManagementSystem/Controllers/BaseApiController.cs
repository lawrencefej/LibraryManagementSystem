using LibraryManagementSystem.Extensions;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController<TDetail, TList> : ControllerBase
    {
        protected virtual IActionResult ResultCheck(LmsResponseHandler<TDetail> result)
        {
            return result.Succeeded ? result.Item != null ? Ok(result.Item) : NoContent() : BadRequest(result.Errors);
        }

        protected virtual IActionResult ReturnPagination(PagedList<TList> items)
        {
            Response.AddPagination(items.CurrentPage, items.PageSize,
                             items.TotalCount, items.TotalPages);

            return Ok(items);
        }
    }
}

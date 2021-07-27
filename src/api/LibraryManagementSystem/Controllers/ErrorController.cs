using System.Collections.Generic;
using LMSEntities.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    // [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Authorize]
        [HttpGet("auth")]
        public IActionResult GetSecret()
        {
            // return Ok("secret text");
            return StatusCode(401, "unauthorize");
        }

        [HttpGet("not-found")]
        [AllowAnonymous]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("server-error")]
        [AllowAnonymous]
        public IActionResult GetServerError()
        {
            return StatusCode(500, "Internal Server Error. Something went Wrong!");
        }

        [HttpGet("bad-request")]
        [AllowAnonymous]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpPost("fluent-validation")]
        [AllowAnonymous]
        public IActionResult GetFluentValidation(UpdateAdminRoleDto adminRoleUpdate)
        {
            return Ok(adminRoleUpdate);
        }

        [HttpPost("annotations")]
        [AllowAnonymous]
        public IActionResult GetAnnotations(AdminUserForListDto adminRoleUpdate)
        {
            return Ok(adminRoleUpdate);
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public IActionResult GetListErrors()
        {
            // return BadRequest(new List<string>() { "Error 1", "Error 2" });
            return BadRequest("string error");
        }
    }
}

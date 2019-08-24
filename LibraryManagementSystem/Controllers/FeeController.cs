using LibraryManagementSystem.API.Helpers;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class FeeController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public FeeController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{librayCardID}")]
        public async Task<IActionResult> Post(int librayCardID)
        {
            await _paymentService.PayFees(librayCardID);

            return Ok();
        }
    }
}
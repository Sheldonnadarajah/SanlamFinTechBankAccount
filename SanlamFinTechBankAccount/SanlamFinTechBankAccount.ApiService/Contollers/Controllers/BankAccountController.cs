using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SanlamFinTechBankAccount.Application.Services;
using SanlamFinTechBankAccount.Presentation.Models;
using SanlamFinTechBankAccount.Application.Models;

namespace SanlamFinTechBankAccount.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _service;

        public BankAccountController(IBankAccountService service)
        {
            _service = service;
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromQuery] long accountId, [FromQuery] decimal amount)
        {
            var result = await _service.WithdrawAsync(accountId, amount);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = result.ErrorCode!,
                    Details = result.Message!
                });
            }
            return Ok(result.Message);
        }
    }
}

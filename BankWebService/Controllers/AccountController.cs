using BankWebService.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DBManager _context;

        public AccountController(DBManager context)
        {
            _context = context;
        }

        // Create Account
        [HttpPost("create_account")]
        public async Task<IActionResult> CreateAccount()
        {
            return NotFound();
        }

        // Get Account by Account Number
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount()
        {
            return NotFound();
        }

        // Update Account
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount()
        {
            return NotFound();
        }

        // Delete Account
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            return NotFound();
        }
    }
}

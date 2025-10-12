using BankWebService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly DBManager _context;

        public TransactionController(DBManager context)
        {
            _context = context;
        }

        // Post: api/withdraw {accountNumber}/{amount}
        [HttpPost("withdraw/{accountNumber}/{amount}")]
        public async Task<ActionResult> Withdraw(int accountNumber, double amount)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(accountNumber);

            if (account == null)
            {
                return NotFound();
            }

            // Add a row to the Transactions table (store transaction history)
            return NotFound();
        }

        // Post: api/deposit/ {accountNumber}/{amount}
        [HttpPost("deposit/{accountNumber}/{amount}")]
        public async Task<ActionResult> Deposit(int accountNumber, double amount)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(accountNumber);

            if (account == null)
            {
                return NotFound();
            }

            // Add a row to the Transactions table (store transaction history)
            return NotFound();
        }
    }
}

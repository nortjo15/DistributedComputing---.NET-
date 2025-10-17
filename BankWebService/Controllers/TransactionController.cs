using BankWebService.Data;
using BankWebService.Models;
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

        // POST: api/Transaction/withdraw/{accountNumber}/{amount}
        [HttpPost("withdraw/{accountNumber}/{amount}")]
        public async Task<ActionResult<Transaction>> Withdraw(int accountNumber, double amount)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts' is null");
            }

            if (_context.Transactions == null)
            {
                return Problem("Entity set 'DBManager.Transactions' is null");
            }

            // Validate amount
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }

            // Get the account
            var account = await _context.Accounts.FindAsync(accountNumber);
            if (account == null)
            {
                return NotFound($"Account {accountNumber} not found");
            }

            // Check sufficient balance
            if (account.Balance < (decimal)amount)
            {
                return BadRequest("Insufficient funds");
            }

            // Update account balance
            account.Balance -= (decimal)amount;
            _context.Entry(account).State = EntityState.Modified;

            // Create a new transaction
            Transaction transaction = new Transaction
            {
                Type = Transaction.TxType.Withdraw,
                AccountNumber = accountNumber,
                Amount = (decimal)amount,
                Account = account
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }

        // POST: api/Transaction/deposit/{accountNumber}/{amount}
        [HttpPost("deposit/{accountNumber}/{amount}")]
        public async Task<ActionResult<Transaction>> Deposit(int accountNumber, double amount)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts' is null");
            }

            if (_context.Transactions == null)
            {
                return Problem("Entity set 'DBManager.Transactions' is null");
            }

            // Validate amount
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }

            // Get the account
            var account = await _context.Accounts.FindAsync(accountNumber);
            if (account == null)
            {
                return NotFound($"Account {accountNumber} not found");
            }

            // Update account balance
            account.Balance += (decimal)amount;
            _context.Entry(account).State = EntityState.Modified;

            // Create a new transaction
            Transaction transaction = new Transaction
            {
                Type = Transaction.TxType.Deposit,
                AccountNumber = accountNumber,
                Amount = (decimal)amount,
                Account = account
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }

        // GET: api/Transaction/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // GET: api/Transaction/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }

            return await _context.Transactions
                                 .AsNoTracking()
                                 .ToListAsync();
        }
    }
}

using BankWebService.Data;
using BankWebService.DTOs;
using BankWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly DBManager _context;
        public TransactionController(DBManager context) => _context = context;

        [HttpPost("withdraw/{accountNumber}/{amount}")]
        public async Task<ActionResult<TransactionDto>> Withdraw(int accountNumber, decimal amount)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
                return NotFound("Account not found");

            // reject invalid amount values
            if (amount <= 0)
                return BadRequest("Amount must be greater than zero");

            // allow overdraft
            account.Balance -= amount;

            var tx = new Transaction
            {
                Type = Transaction.TxType.Withdraw,
                AccountNumber = accountNumber,
                Amount = amount,
                Account = account
            };

            _context.Transactions.Add(tx);
            await _context.SaveChangesAsync();

            return Ok(new TransactionDto
            {
                TransactionId = tx.TransactionId,
                AccountNumber = tx.AccountNumber,
                Type = tx.Type.ToString(),
                Amount = tx.Amount
            });
        }


        [HttpPost("deposit/{accountNumber}/{amount}")]
        public async Task<ActionResult<TransactionDto>> Deposit(int accountNumber, decimal amount)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
                return NotFound("Account not found");

            // reject invalid amount values
            if (amount <= 0)
                return BadRequest("Amount must be greater than zero");

            account.Balance += amount;

            var tx = new Transaction
            {
                Type = Transaction.TxType.Deposit,
                AccountNumber = accountNumber,
                Amount = amount,
                Account = account
            };

            _context.Transactions.Add(tx);
            await _context.SaveChangesAsync();

            return Ok(new TransactionDto
            {
                TransactionId = tx.TransactionId,
                AccountNumber = tx.AccountNumber,
                Type = tx.Type.ToString(),
                Amount = tx.Amount
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var t = await _context.Transactions.FindAsync(id);
            if (t == null) return NotFound();

            return new TransactionDto
            {
                TransactionId = t.TransactionId,
                AccountNumber = t.AccountNumber,
                Type = t.Type.ToString(),
                Amount = t.Amount
            };
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
        {
            var list = await _context.Transactions
                .Select(t => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    AccountNumber = t.AccountNumber,
                    Type = t.Type.ToString(),
                    Amount = t.Amount
                })
                .AsNoTracking()
                .ToListAsync();

            return list;
        }
    }
}

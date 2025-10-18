using BankWebService.Data;
using BankWebService.DTOs;
using BankWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DBManager _context;
        public AccountController(DBManager context) => _context = context;

        [HttpPost("create_account")]
        public async Task<ActionResult<AccountDto>> CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var dto = new AccountDto
            {
                AccountNumber = account.AccountNumber,
                Name = account.Name,
                Balance = account.Balance,
                Username = account.Username,
                Email = account.Email
            };

            return CreatedAtAction(nameof(GetAccount), new { accountNumber = account.AccountNumber }, dto);
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int accountNumber)
        {
            var a = await _context.Accounts.FindAsync(accountNumber);
            if (a == null) return NotFound();

            return new AccountDto
            {
                AccountNumber = a.AccountNumber,
                Name = a.Name,
                Balance = a.Balance,
                Username = a.Username,
                Email = a.Email
            };
        }

        [HttpPut("{accountNumber}")]
        public async Task<IActionResult> UpdateAccount(int accountNumber, Account account)
        {
            if (accountNumber != account.AccountNumber) return BadRequest();

            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{accountNumber}")]
        public async Task<IActionResult> DeleteAccount(int accountNumber)
        {
            var account = await _context.Accounts.FindAsync(accountNumber);
            if (account == null) return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var list = await _context.Accounts
                .Select(a => new AccountDto
                {
                    AccountNumber = a.AccountNumber,
                    Name = a.Name,
                    Balance = a.Balance,
                    Username = a.Username,
                    Email = a.Email
                })
                .AsNoTracking()
                .ToListAsync();

            return list;
        }
    }
}

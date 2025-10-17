using BankWebService.Data;
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

        public AccountController(DBManager context)
        {
            _context = context;
        }

        // Create Account // TODO
        // Post: api/account/create_account
        [HttpPost("create_account")]
        public async Task<ActionResult<Account>> CreateAccount(Account account)
        {
            if(_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts'  is null.");
            }
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAccount", new { accountNumber = account.AccountNumber }, account );
        }

        // Get Account by Account Number
        // GET: api/account/{accountNumber}
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<Account>> GetAccount(int accountNumber)
        {
            if(_context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(accountNumber);
            if(account == null)
            {
                return NotFound();
            }

            return account;
        }

        // Update Account
        // PUT: api/account/{accountNumber}
        [HttpPut("{accountNumber}")]
        public async Task<IActionResult> UpdateAccount(int accountNumber, Account account)
        {
            if(accountNumber != account.AccountNumber)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!AccountExists(accountNumber))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete Account
        // DELETE: api/account/{accountNumber}
        [HttpDelete("{accountNumber}")]
        public async Task<IActionResult> DeleteAccount(int accountNumber)
        {
            if(_context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(accountNumber);
            if(account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/account/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }

            return await _context.Accounts
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountNumber == id)).GetValueOrDefault();
        }
    }
}

using BankWebService.Data;
using BankWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly DBManager _context;

        public UserProfileController(DBManager context)
        {
            _context = context;
        }

        // List all User Profiles
        // GET: api/userprofile/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetAll()
        {
            if (_context.UserProfiles == null)
                return NotFound();
            return await _context.UserProfiles.AsNoTracking().ToListAsync();
        }

        // Create User Profile
        // POST: api/userprofile/create_profile
        [HttpPost("create_profile")]
        public async Task<ActionResult<UserProfile>> CreateProfile(UserProfile profile)
        {
            if (_context.UserProfiles == null)
            {
                return Problem("Entity set 'DBManager.UserProfiles'  is null.");
            }
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProfileByUsername", new { username = profile.Username }, profile);
        }

        // Get User Profile By Username
        // GET: api/userprofile/by-username/{username}
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UserProfile>> GetProfileByUsername(string username)
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            var account = await _context.UserProfiles.FindAsync(username);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // Get User Profile By Email
        // GET: api/userprofile/by-email/{email}
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<UserProfile>> GetProfileByEmail(string email)
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            var account = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // Update User Profile
        // PUT: api/userprofile/{username}
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, UserProfile profile)
        {
            if (username != profile.Username)
            {
                return BadRequest();
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(username))
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

        // Delete User Profile
        // DELETE: api/userprofile/{username}
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            var profile = await _context.UserProfiles.FindAsync(username);
            if (profile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(string username)
        {
            return (_context.UserProfiles?.Any(e => e.Username == username)).GetValueOrDefault();
        }
    }
}

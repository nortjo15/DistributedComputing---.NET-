using BankWebService.Data;
using BankWebService.DTOs;
using BankWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly DBManager _context;
        public UserProfileController(DBManager context) => _context = context;

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAll()
        {
            var profiles = await _context.UserProfiles
                .Select(p => new UserProfileDto
                {
                    Username = p.Username,
                    Email = p.Email,
                    Address = p.Address,
                    Phone = p.Phone,
                    Picture = p.Picture
                })
                .AsNoTracking()
                .ToListAsync();

            return profiles;
        }

        [HttpPost("create_profile")]
        public async Task<ActionResult<UserProfileDto>> CreateProfile(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            var dto = new UserProfileDto
            {
                Username = profile.Username,
                Email = profile.Email,
                Address = profile.Address,
                Phone = profile.Phone,
                Picture = profile.Picture
            };

            return CreatedAtAction(nameof(GetProfileByUsername), new { username = profile.Username }, dto);
        }

        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UserProfileDto>> GetProfileByUsername(string username)
        {
            var p = await _context.UserProfiles.FindAsync(username);
            if (p == null) return NotFound();

            return new UserProfileDto
            {
                Username = p.Username,
                Email = p.Email,
                Address = p.Address,
                Phone = p.Phone,
                Picture = p.Picture,
                Password = p.Password // Include password for authentication purposes
            };
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<UserProfileDto>> GetProfileByEmail(string email)
        {
            var p = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);
            if (p == null) return NotFound();

            return new UserProfileDto
            {
                Username = p.Username,
                Email = p.Email,
                Address = p.Address,
                Phone = p.Phone,
                Picture = p.Picture
            };
        }

        [HttpGet("by-phone/{phone}")]
        public async Task<ActionResult<UserProfileDto>> GetProfileByPhone(string phone)
        {
            var p = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Phone == phone);
            if (p == null) return NotFound();

            return new UserProfileDto
            {
                Username = p.Username,
                Email = p.Email,
                Address = p.Address,
                Phone = p.Phone,
                Picture = p.Picture
            };
        }

        // Change password
        // PUT: api/userprofile/{username}/password
        [HttpPut("{username}/password")]
        public async Task<IActionResult> ChangePassword(string username, [FromBody] ChangePasswordRequest request)
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            if (request == null || !string.Equals(username, request.Username, StringComparison.Ordinal))
            {
                return BadRequest("Username mismatch or empty request");
            }

            var user = await _context.UserProfiles.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            // Optional current password check (if provided)
            if (!string.IsNullOrEmpty(request.CurrentPassword) && !string.Equals(user.Password, request.CurrentPassword))
            {
                return BadRequest("Current password is incorrect");
            }

            // Basic length validation already enforced by model, but double-check here
            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length > 30)
            {
                return BadRequest("New password invalid");
            }

            user.Password = request.NewPassword;
            _context.Entry(user).Property(u => u.Password).IsModified = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

using BankWebService.Data;
using BankWebService.Models;
using Microsoft.AspNetCore.Mvc;

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

        // Create User Profile
        [HttpPost("create_profile")]
        public async Task<IActionResult> CreateProfile()
        {
            return NotFound();
        }

        // Get User Profile By Username
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfileByUsername(string username)
        {
            return NotFound();
        }

        // Get User Profile By Email
        [HttpGet("{email}")]
        public async Task<IActionResult> GetProfileByEmail(string email)
        {
            return NotFound();
        }

        // Update User Profile
        [HttpPut("{accountNumber}")]
        public async Task<IActionResult> UpdateUser(string username)
        {
            return NotFound();
        }

        // Delete User Profile
        [HttpDelete("{accountNumber}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            return NotFound();
        }
    }
}

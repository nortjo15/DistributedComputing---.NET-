using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BankWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IHttpClientFactory clientFactory, ILogger<AdminController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("BankApi");
            var users = await client.GetFromJsonAsync<List<UserProfileDto>>("api/userprofile/all");
            return View(users ?? new List<UserProfileDto>());
        }
    }

    public class UserProfileDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}

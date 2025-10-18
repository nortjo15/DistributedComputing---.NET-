using BankWebApp.Models;
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

            // fetch users, transactions, accounts concurrently
            var usersTask = client.GetFromJsonAsync<List<UserProfileDto>>("api/userprofile/all");
            var txTask = client.GetFromJsonAsync<List<TransactionDto>>("api/transaction/all");
            var accountsTask = client.GetFromJsonAsync<List<AccountDto>>("api/account/all");

            // fetch admin by username – try BankAdmin first
            var adminTask = client.GetFromJsonAsync<UserProfileDto>("api/userprofile/by-username/BankAdmin");

            try
            {
                await Task.WhenAll(usersTask!, txTask!, accountsTask!, adminTask!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch data for admin dashboard");
            }

            var users = usersTask?.Status == TaskStatus.RanToCompletion && usersTask.Result != null
                ? usersTask.Result
                : new List<UserProfileDto>();

            // If explicit BankAdmin call failed, attempt to find a user containing 'admin'
            UserProfileDto? admin = null;
            if (adminTask?.Status == TaskStatus.RanToCompletion && adminTask.Result != null)
            {
                admin = adminTask.Result;
            }
            else
            {
                admin = users.FirstOrDefault(u => u.Username.Contains("admin", StringComparison.OrdinalIgnoreCase));
            }

            var model = new AdminDashboardViewModel
            {
                Users = users,
                Transactions = txTask?.Status == TaskStatus.RanToCompletion && txTask.Result != null
                    ? txTask.Result
                    : new List<TransactionDto>(),
                Accounts = accountsTask?.Status == TaskStatus.RanToCompletion && accountsTask.Result != null
                    ? accountsTask.Result
                    : new List<AccountDto>(),
                Admin = admin
            };

            return View(model);
        }

        // C - Create account
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest req)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            var client = _clientFactory.CreateClient("BankApi");

            var account = new AccountDto
            {
                // AccountNumber is DB-generated (autoincrement)
                Username = req.Username,
                Email = req.Email,
                Balance = req.Balance
            };

            try
            {
                var resp = await client.PostAsJsonAsync("api/account/create_account", account);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("CreateAccount failed: {Status}", resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAccount error");
            }

            return RedirectToAction(nameof(Index));
        }

        // R - Get account by number (simple redirect to Index after fetching is omitted, index already lists all)
        [HttpGet]
        public async Task<IActionResult> GetAccount(int accountNumber)
        {
            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var account = await client.GetFromJsonAsync<AccountDto>($"api/account/{accountNumber}");
                TempData["SelectedAccount"] = accountNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAccount error for {AccountNumber}", accountNumber);
            }
            return RedirectToAction(nameof(Index));
        }

        // U - Update account
        [HttpPost]
        public async Task<IActionResult> UpdateAccount(AccountDto account)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var resp = await client.PutAsJsonAsync($"api/account/{account.AccountNumber}", account);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("UpdateAccount failed: {Status}", resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAccount error for {AccountNumber}", account.AccountNumber);
            }
            return RedirectToAction(nameof(Index));
        }

        // D - Delete account
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int accountNumber)
        {
            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var resp = await client.DeleteAsync($"api/account/{accountNumber}");
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("DeleteAccount failed: {Status}", resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAccount error for {AccountNumber}", accountNumber);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

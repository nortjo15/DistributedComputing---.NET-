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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest req)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = string.IsNullOrWhiteSpace(errors) ? "Invalid form input." : errors;
                _logger.LogWarning("CreateAccount invalid: {Errors}", TempData["Error"]);
                return RedirectToAction(nameof(Index));
            }

            var client = _clientFactory.CreateClient("BankApi");

            // Ensure user exists first (FK constraint)
            try
            {
                var userCheck = await client.GetAsync($"api/userprofile/by-username/{Uri.EscapeDataString(req.Username)}");
                if (!userCheck.IsSuccessStatusCode)
                {
                    TempData["Error"] = $"User '{req.Username}' does not exist. Create the user profile first.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to verify user '{req.Username}': {ex.Message}";
                _logger.LogError(ex, "User existence check failed");
                return RedirectToAction(nameof(Index));
            }

            var account = new AccountDto
            {
                Username = req.Username,
                Email = req.Email,
                Balance = req.Balance
            };

            try
            {
                var resp = await client.PostAsJsonAsync("api/account/create_account", account);
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    var msg = $"CreateAccount failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}";
                    TempData["Error"] = msg;
                    _logger.LogWarning(msg);
                }
                else
                {
                    TempData["Message"] = "Account created.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"CreateAccount error: {ex.Message}";
                _logger.LogError(ex, "CreateAccount error");
            }

            return RedirectToAction(nameof(Index));
        }

        // R - Get account by number (optional: highlight on page)
        [HttpGet]
        public async Task<IActionResult> GetAccount(int accountNumber)
        {
            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var account = await client.GetFromJsonAsync<AccountDto>($"api/account/{accountNumber}");
                if (account == null)
                {
                    TempData["Error"] = $"Account {accountNumber} not found.";
                }
                else
                {
                    TempData["Message"] = $"Fetched account {accountNumber}.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"GetAccount error: {ex.Message}";
                _logger.LogError(ex, "GetAccount error for {AccountNumber}", accountNumber);
            }
            return RedirectToAction(nameof(Index));
        }

        // U - Update account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(AccountDto account)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = string.IsNullOrWhiteSpace(errors) ? "Invalid form input." : errors;
                _logger.LogWarning("UpdateAccount invalid: {Errors}", TempData["Error"]);
                return RedirectToAction(nameof(Index));
            }

            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var resp = await client.PutAsJsonAsync($"api/account/{account.AccountNumber}", account);
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    var msg = $"UpdateAccount failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}";
                    TempData["Error"] = msg;
                    _logger.LogWarning(msg);
                }
                else
                {
                    TempData["Message"] = $"Account {account.AccountNumber} updated.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"UpdateAccount error: {ex.Message}";
                _logger.LogError(ex, "UpdateAccount error for {AccountNumber}", account.AccountNumber);
            }
            return RedirectToAction(nameof(Index));
        }

        // D - Delete account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(int accountNumber)
        {
            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var resp = await client.DeleteAsync($"api/account/{accountNumber}");
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    var msg = $"DeleteAccount failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}";
                    TempData["Error"] = msg;
                    _logger.LogWarning(msg);
                }
                else
                {
                    TempData["Message"] = $"Account {accountNumber} deleted.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"DeleteAccount error: {ex.Message}";
                _logger.LogError(ex, "DeleteAccount error for {AccountNumber}", accountNumber);
            }
            return RedirectToAction(nameof(Index));
        }

        // Change a user's password (from modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordRequest req)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = string.IsNullOrWhiteSpace(errors) ? "Invalid password form input." : errors;
                return RedirectToAction(nameof(Index));
            }

            var client = _clientFactory.CreateClient("BankApi");
            try
            {
                var resp = await client.PutAsJsonAsync($"api/userprofile/{Uri.EscapeDataString(req.Username)}/password", req);
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Change password failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}";
                }
                else
                {
                    TempData["Message"] = $"Password updated for {req.Username}.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeUserPassword error for {Username}", req.Username);
                TempData["Error"] = $"ChangeUserPassword error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

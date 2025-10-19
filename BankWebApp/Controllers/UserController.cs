using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BankWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<UserController> _logger;

        public UserController(IHttpClientFactory clientFactory, ILogger<UserController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await GetView();
        }

        public async Task<IActionResult> GetView()
        {
            var client = _clientFactory.CreateClient("BankApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "dummy-token");

            try
            {
                var username = Request.Cookies["Username"];

                var profileTask = client.GetFromJsonAsync<UserProfileDto>($"api/UserProfile/by-username/{username}");
                var accountsTask = client.GetFromJsonAsync<List<AccountDto>>("api/Account/all");
                var transactionsTask = client.GetFromJsonAsync<List<TransactionDto>>("api/Transaction/all");
                var profilesTask = client.GetFromJsonAsync<List<UserProfileDto>>("api/UserProfile/all");
                
                await Task.WhenAll(profileTask!, accountsTask!, transactionsTask!, profilesTask!);

                var model = new UserDashboardViewModel
                {
                    Profile = profileTask.Result,
                    Accounts = accountsTask.Result ?? new List<AccountDto>(),
                    Transactions = transactionsTask.Result ?? new List<TransactionDto>(),
                    Profiles = profilesTask.Result ?? new List<UserProfileDto>()
                };

                return PartialView("UserDashboardView", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API fetch failed");
                
                var emptyModel = new UserDashboardViewModel
                {
                    Profile = null,
                    Accounts = new List<AccountDto>(),
                    Transactions = new List<TransactionDto>(),
                    Profiles = new List<UserProfileDto>()
                };
                
                return PartialView("UserDashboardView", emptyModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int FromAccountId, int ToAccountId, decimal Amount)
        {
            var client = _clientFactory.CreateClient("BankApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "dummy-token");

            try
            {
                _logger.LogInformation($"Withdraw {FromAccountId}, Deposit {ToAccountId}, Amount {Amount}");

                var withdrawResponse = await client.PostAsync(
                    $"api/Transaction/withdraw/{FromAccountId}/{Amount}", null);
                _logger.LogInformation($"Withdraw status: {withdrawResponse.StatusCode}");

                var withdrawContent = await withdrawResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Withdraw response: {withdrawContent}");

                withdrawResponse.EnsureSuccessStatusCode();

                var depositResponse = await client.PostAsync(
                    $"api/Transaction/deposit/{ToAccountId}/{Amount}", null);
                _logger.LogInformation($"Deposit status: {depositResponse.StatusCode}");

                var depositContent = await depositResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Deposit response: {depositContent}");

                depositResponse.EnsureSuccessStatusCode();

                TempData["Message"] = "Transfer completed successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transfer failed");
                TempData["Message"] = $"Transfer failed: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Security() => PartialView();
    }
}

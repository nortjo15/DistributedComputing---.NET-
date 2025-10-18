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

        public async Task<IActionResult> GetView()
        {
            var client = _clientFactory.CreateClient("BankApi");

            // fetch users, transactions, accounts concurrently
            var usersTask = client.GetFromJsonAsync<List<UserProfileDto>>("api/userprofile/all");
            var txTask = client.GetFromJsonAsync<List<TransactionDto>>("api/transaction/all");
            var accountsTask = client.GetFromJsonAsync<List<AccountDto>>("api/account/all");

            try
            {
                await Task.WhenAll(usersTask!, txTask!, accountsTask!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch data for admin dashboard");
            }

            var model = new AdminDashboardViewModel
            {
                Users = usersTask?.Status == TaskStatus.RanToCompletion && usersTask.Result != null
                    ? usersTask.Result
                    : new List<UserProfileDto>(),
                Transactions = txTask?.Status == TaskStatus.RanToCompletion && txTask.Result != null
                    ? txTask.Result
                    : new List<TransactionDto>(),
                Accounts = accountsTask?.Status == TaskStatus.RanToCompletion && accountsTask.Result != null
                    ? accountsTask.Result
                    : new List<AccountDto>()
            };

            return PartialView(model);
        }
    }
}

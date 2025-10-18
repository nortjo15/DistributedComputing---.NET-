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
        public async Task<IActionResult> GetView()
        {
            var client = _clientFactory.CreateClient("BankApi");

            // Placeholder token until authentication implemented
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "dummy-token");


            try
            {
                var profileTask = client.GetFromJsonAsync<UserProfileDto>("api/UserProfile/by-username/barbaraparker204");
                var accountsTask = client.GetFromJsonAsync<List<AccountDto>>("api/account/all");
                var transactionsTask = client.GetFromJsonAsync<List<TransactionDto>>("api/transaction/all");
                await Task.WhenAll(profileTask!, accountsTask!, transactionsTask!);

                var model = new UserDashboardViewModel
                {
                    Profile = profileTask.Result,
                    Accounts = accountsTask.Result,
                    Transactions = transactionsTask.Result
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API fetch failed");
                return PartialView(new UserDashboardViewModel());
            }
        }

        public IActionResult Transfer() => PartialView();
        public IActionResult Security() => PartialView();

    }

}

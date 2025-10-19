using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BankWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly RestClient _client;

        public UserController()
        {
            _client = new RestClient("http://localhost:5142");
        }

        public async Task<IActionResult> Index()
        {
            return await GetView();
        }

        public async Task<IActionResult> GetView()
        {
            try
            {
                var username = Request.Cookies["Username"];

                var profileRequest = new RestRequest($"/api/UserProfile/by-username/{username}");
                var accountsRequest = new RestRequest("/api/Account/all");
                var transactionsRequest = new RestRequest("/api/Transaction/all");
                var profilesRequest = new RestRequest("/api/UserProfile/all");

                var profileTask = _client.ExecuteAsync(profileRequest);
                var accountsTask = _client.ExecuteAsync(accountsRequest);
                var transactionsTask = _client.ExecuteAsync(transactionsRequest);
                var profilesTask = _client.ExecuteAsync(profilesRequest);
                
                await Task.WhenAll(profileTask, accountsTask, transactionsTask, profilesTask);

                var profile = profileTask.Result.IsSuccessful && profileTask.Result.Content != null
                    ? JsonConvert.DeserializeObject<UserProfileDto>(profileTask.Result.Content)
                    : null;

                var accounts = accountsTask.Result.IsSuccessful && accountsTask.Result.Content != null
                    ? JsonConvert.DeserializeObject<List<AccountDto>>(accountsTask.Result.Content) ?? new List<AccountDto>()
                    : new List<AccountDto>();

                var transactions = transactionsTask.Result.IsSuccessful && transactionsTask.Result.Content != null
                    ? JsonConvert.DeserializeObject<List<TransactionDto>>(transactionsTask.Result.Content) ?? new List<TransactionDto>()
                    : new List<TransactionDto>();

                var profiles = profilesTask.Result.IsSuccessful && profilesTask.Result.Content != null
                    ? JsonConvert.DeserializeObject<List<UserProfileDto>>(profilesTask.Result.Content) ?? new List<UserProfileDto>()
                    : new List<UserProfileDto>();

                var model = new UserDashboardViewModel
                {
                    Profile = profile,
                    Accounts = accounts,
                    Transactions = transactions,
                    Profiles = profiles
                };

                return PartialView("UserDashboardView", model);
            }
            catch (Exception)
            {
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
            try
            {
                var withdrawRequest = new RestRequest($"/api/Transaction/withdraw/{FromAccountId}/{Amount}", Method.Post);
                var withdrawResponse = await _client.ExecuteAsync(withdrawRequest);

                withdrawResponse.ThrowIfError();

                var depositRequest = new RestRequest($"/api/Transaction/deposit/{ToAccountId}/{Amount}", Method.Post);
                var depositResponse = await _client.ExecuteAsync(depositRequest);

                depositResponse.ThrowIfError();

                return Json(new { success = true, message = "Transfer completed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Transfer failed: {ex.Message}" });
            }
        }

        public IActionResult Security() => PartialView();
    }
}

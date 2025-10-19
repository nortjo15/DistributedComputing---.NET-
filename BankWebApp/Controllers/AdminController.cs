using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BankWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly RestClient _client;

        public AdminController()
        {
            _client = new RestClient("http://localhost:5142");
        }

        public async Task<IActionResult> Index()
        {
            return await GetView();
        }

        public async Task<IActionResult> GetView()
        {
            // fetch users, transactions, accounts concurrently
            var usersRequest = new RestRequest("/api/userprofile/all");
            var txRequest = new RestRequest("/api/transaction/all");
            var accountsRequest = new RestRequest("/api/account/all");
            var adminRequest = new RestRequest("/api/userprofile/by-username/BankAdmin");

            var usersTask = _client.ExecuteAsync(usersRequest);
            var txTask = _client.ExecuteAsync(txRequest);
            var accountsTask = _client.ExecuteAsync(accountsRequest);
            var adminTask = _client.ExecuteAsync(adminRequest);

            try
            {
                await Task.WhenAll(usersTask, txTask, accountsTask, adminTask);
            }
            catch (Exception)
            {
                // Failed to fetch data for admin dashboard
            }

            var users = usersTask.Result.IsSuccessful && usersTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<UserProfileDto>>(usersTask.Result.Content) ?? new List<UserProfileDto>()
                : new List<UserProfileDto>();

            var transactions = txTask.Result.IsSuccessful && txTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<TransactionDto>>(txTask.Result.Content) ?? new List<TransactionDto>()
                : new List<TransactionDto>();

            var accounts = accountsTask.Result.IsSuccessful && accountsTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<AccountDto>>(accountsTask.Result.Content) ?? new List<AccountDto>()
                : new List<AccountDto>();

            // If explicit BankAdmin call failed, attempt to find a user containing 'admin'
            UserProfileDto? admin = null;
            if (adminTask.Result.IsSuccessful && adminTask.Result.Content != null)
            {
                admin = JsonConvert.DeserializeObject<UserProfileDto>(adminTask.Result.Content);
            }
            else
            {
                admin = users.FirstOrDefault(u => u.Username.Contains("admin", StringComparison.OrdinalIgnoreCase));
            }

            var model = new AdminDashboardViewModel
            {
                Users = users,
                Transactions = transactions,
                Accounts = accounts,
                Admin = admin
            };

            return View("AdminDashboardView", model);
        }

        // C - Create account
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest req)
        {
            if (req == null)
            {
                TempData["Error"] = "Invalid request data";
                return await GetView();
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = string.IsNullOrWhiteSpace(errors) ? "Invalid form input." : errors;
                return await GetView();
            }

            // Ensure user exists first (FK constraint)
            try
            {
                var userCheckRequest = new RestRequest($"/api/userprofile/by-username/{Uri.EscapeDataString(req.Username)}");
                var userCheckResponse = await _client.ExecuteAsync(userCheckRequest);
                
                if (!userCheckResponse.IsSuccessful)
                {
                    TempData["Error"] = $"User '{req.Username}' does not exist. Create the user profile first.";
                    return await GetView();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to verify user '{req.Username}': {ex.Message}";
                return await GetView();
            }

            // Create account request
            var accountRequest = new
            {
                Name = req.Name,
                Username = req.Username,
                Email = req.Email,
                Balance = req.Balance
            };

            try
            {
                var request = new RestRequest("/api/account/create_account", Method.Post);
                request.AddJsonBody(accountRequest);
                
                var response = await _client.ExecuteAsync(request);
                
                if (!response.IsSuccessful)
                {
                    var msg = $"CreateAccount failed: {(int)response.StatusCode} {response.StatusDescription}. {response.Content}";
                    TempData["Error"] = msg;
                }
                else
                {
                    TempData["Message"] = "Account created successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"CreateAccount error: {ex.Message}";
            }

            return await GetView();
        }

        // R - Get account by number (optional: highlight on page)
        [HttpGet]
        public async Task<IActionResult> GetAccount(int accountNumber)
        {
            try
            {
                var request = new RestRequest($"/api/account/{accountNumber}");
                var response = await _client.ExecuteAsync(request);
                
                if (!response.IsSuccessful || response.Content == null)
                {
                    TempData["Error"] = $"Account {accountNumber} not found.";
                }
                else
                {
                    var account = JsonConvert.DeserializeObject<AccountDto>(response.Content);
                    if (account == null)
                    {
                        TempData["Error"] = $"Account {accountNumber} not found.";
                    }
                    else
                    {
                        TempData["Message"] = $"Fetched account {accountNumber}.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"GetAccount error: {ex.Message}";
            }
            return await GetView();
        }

        // U - Update account
        [HttpPost]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountDto account)
        {
            if (account == null)
            {
                TempData["Error"] = "Invalid account data";
                return await GetView();
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = string.IsNullOrWhiteSpace(errors) ? "Invalid form input." : errors;
                return await GetView();
            }

            try
            {
                var request = new RestRequest($"/api/account/{account.AccountNumber}", Method.Put);
                request.AddJsonBody(account);
                
                var response = await _client.ExecuteAsync(request);
                
                if (!response.IsSuccessful)
                {
                    var msg = $"UpdateAccount failed: {(int)response.StatusCode} {response.StatusDescription}. {response.Content}";
                    TempData["Error"] = msg;
                }
                else
                {
                    TempData["Message"] = $"Account {account.AccountNumber} updated successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"UpdateAccount error: {ex.Message}";
            }
            return await GetView();
        }

        // D - Delete account
        [HttpPost]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest req)
        {
            if (req == null || req.AccountNumber <= 0)
            {
                TempData["Error"] = "Invalid account number";
                return await GetView();
            }
            
            try
            {
                var request = new RestRequest($"/api/account/{req.AccountNumber}", Method.Delete);
                var response = await _client.ExecuteAsync(request);
                
                if (!response.IsSuccessful)
                {
                    var msg = $"DeleteAccount failed: {(int)response.StatusCode} {response.StatusDescription}. {response.Content}";
                    TempData["Error"] = msg;
                }
                else
                {
                    TempData["Message"] = $"Account {req.AccountNumber} deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"DeleteAccount error: {ex.Message}";
            }
            return await GetView();
        }

        // Change a user's password (from modal)
        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword([FromForm] ChangePasswordRequest req)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = string.IsNullOrWhiteSpace(errors) ? "Invalid password form input." : errors });
            }

            try
            {
                var request = new RestRequest($"/api/userprofile/{Uri.EscapeDataString(req.Username)}/password", Method.Put);
                request.AddJsonBody(req);
                
                var response = await _client.ExecuteAsync(request);
                
                if (!response.IsSuccessful)
                {
                    var msg = $"Change password failed: {(int)response.StatusCode} {response.StatusDescription}. {response.Content}";
                    return Json(new { success = false, message = msg });
                }
                else
                {
                    return Json(new { success = true, message = $"Password updated for {req.Username}." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"ChangeUserPassword error: {ex.Message}" });
            }
        }

        // Search users by username or email
        [HttpPost]
        public async Task<IActionResult> SearchUsers([FromBody] SearchUsersRequest req)
        {
            if (req == null || (string.IsNullOrWhiteSpace(req.Username) && string.IsNullOrWhiteSpace(req.Email) && string.IsNullOrWhiteSpace(req.Phone)))
            {
                TempData["Error"] = "Please provide username, email, or phone number to search";
                return await GetView();
            }

            // Check if multiple search criteria provided
            int searchCriteriaCount = 0;
            if (!string.IsNullOrWhiteSpace(req.Username)) searchCriteriaCount++;
            if (!string.IsNullOrWhiteSpace(req.Email)) searchCriteriaCount++;
            if (!string.IsNullOrWhiteSpace(req.Phone)) searchCriteriaCount++;

            if (searchCriteriaCount > 1)
            {
                TempData["Error"] = "Please search by only one criteria at a time";
                return await GetView();
            }

            List<UserProfileDto> searchResults = new();

            try
            {
                UserProfileDto? user = null;
                RestRequest request;

                if (!string.IsNullOrWhiteSpace(req.Username))
                {
                    request = new RestRequest($"/api/userprofile/by-username/{Uri.EscapeDataString(req.Username)}");
                    var response = await _client.ExecuteAsync(request);
                    if (response.IsSuccessful && response.Content != null)
                    {
                        user = JsonConvert.DeserializeObject<UserProfileDto>(response.Content);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(req.Email))
                {
                    request = new RestRequest($"/api/userprofile/by-email/{Uri.EscapeDataString(req.Email)}");
                    var response = await _client.ExecuteAsync(request);
                    if (response.IsSuccessful && response.Content != null)
                    {
                        user = JsonConvert.DeserializeObject<UserProfileDto>(response.Content);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(req.Phone))
                {
                    request = new RestRequest($"/api/userprofile/by-phone/{Uri.EscapeDataString(req.Phone)}");
                    var response = await _client.ExecuteAsync(request);
                    if (response.IsSuccessful && response.Content != null)
                    {
                        user = JsonConvert.DeserializeObject<UserProfileDto>(response.Content);
                    }
                }

                if (user != null)
                {
                    searchResults.Add(user);
                    TempData["Message"] = $"Found user: {user.Username}";
                }
                else
                {
                    TempData["Error"] = "No user found with the specified criteria";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Search error: {ex.Message}";
            }

            // Get other data for the dashboard
            var txRequest = new RestRequest("/api/transaction/all");
            var accountsRequest = new RestRequest("/api/account/all");
            var adminRequest = new RestRequest("/api/userprofile/by-username/BankAdmin");

            var txTask = _client.ExecuteAsync(txRequest);
            var accountsTask = _client.ExecuteAsync(accountsRequest);
            var adminTask = _client.ExecuteAsync(adminRequest);

            try
            {
                await Task.WhenAll(txTask, accountsTask, adminTask);
            }
            catch (Exception)
            {
                // Failed to fetch additional data for search results
            }

            var transactions = txTask.Result.IsSuccessful && txTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<TransactionDto>>(txTask.Result.Content) ?? new List<TransactionDto>()
                : new List<TransactionDto>();

            var accounts = accountsTask.Result.IsSuccessful && accountsTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<AccountDto>>(accountsTask.Result.Content) ?? new List<AccountDto>()
                : new List<AccountDto>();

            // Get admin info
            UserProfileDto? admin = null;
            if (adminTask.Result.IsSuccessful && adminTask.Result.Content != null)
            {
                admin = JsonConvert.DeserializeObject<UserProfileDto>(adminTask.Result.Content);
            }

            var model = new AdminDashboardViewModel
            {
                Users = searchResults,
                Transactions = transactions,
                Accounts = accounts,
                Admin = admin
            };

            return PartialView("AdminDashboardView", model);
        }

        // Search accounts by account number
        [HttpPost]
        public async Task<IActionResult> SearchAccounts([FromBody] SearchAccountsRequest req)
        {
            if (req == null || (!req.AccountNumber.HasValue && string.IsNullOrWhiteSpace(req.Username) && string.IsNullOrWhiteSpace(req.Email)))
            {
                TempData["Error"] = "Please provide account number, username, or email to search";
                return await GetView();
            }

            // Check if multiple search criteria provided
            int searchCriteriaCount = 0;
            if (req.AccountNumber.HasValue && req.AccountNumber > 0) searchCriteriaCount++;
            if (!string.IsNullOrWhiteSpace(req.Username)) searchCriteriaCount++;
            if (!string.IsNullOrWhiteSpace(req.Email)) searchCriteriaCount++;

            if (searchCriteriaCount > 1)
            {
                TempData["Error"] = "Please search by only one criteria at a time";
                return await GetView();
            }

            List<AccountDto> searchResults = new();

            try
            {
                if (req.AccountNumber.HasValue && req.AccountNumber > 0)
                {
                    // Search by account number
                    var request = new RestRequest($"/api/account/{req.AccountNumber}");
                    var response = await _client.ExecuteAsync(request);
                    
                    if (response.IsSuccessful && response.Content != null)
                    {
                        var account = JsonConvert.DeserializeObject<AccountDto>(response.Content);
                        if (account != null)
                        {
                            searchResults.Add(account);
                            TempData["Message"] = $"Found account: {account.AccountNumber}";
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"No account found with number {req.AccountNumber}";
                    }
                }
                else
                {
                    // Search by username or email - get all accounts and filter
                    var allAccountsRequest = new RestRequest("/api/account/all");
                    var allAccountsResponse = await _client.ExecuteAsync(allAccountsRequest);
                    
                    if (allAccountsResponse.IsSuccessful && allAccountsResponse.Content != null)
                    {
                        var allAccounts = JsonConvert.DeserializeObject<List<AccountDto>>(allAccountsResponse.Content);
                        if (allAccounts != null)
                        {
                            if (!string.IsNullOrWhiteSpace(req.Username))
                            {
                                searchResults = allAccounts.Where(a => a.Username.Equals(req.Username, StringComparison.OrdinalIgnoreCase)).ToList();
                                if (searchResults.Any())
                                {
                                    TempData["Message"] = $"Found {searchResults.Count} account(s) for username: {req.Username}";
                                }
                                else
                                {
                                    TempData["Error"] = $"No accounts found for username: {req.Username}";
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(req.Email))
                            {
                                searchResults = allAccounts.Where(a => !string.IsNullOrEmpty(a.Email) && a.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase)).ToList();
                                if (searchResults.Any())
                                {
                                    TempData["Message"] = $"Found {searchResults.Count} account(s) for email: {req.Email}";
                                }
                                else
                                {
                                    TempData["Error"] = $"No accounts found for email: {req.Email}";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Search error: {ex.Message}";
            }

            // Get other data for the dashboard
            var usersRequest = new RestRequest("/api/userprofile/all");
            var txRequest = new RestRequest("/api/transaction/all");
            var adminRequest = new RestRequest("/api/userprofile/by-username/BankAdmin");

            var usersTask = _client.ExecuteAsync(usersRequest);
            var txTask = _client.ExecuteAsync(txRequest);
            var adminTask = _client.ExecuteAsync(adminRequest);

            try
            {
                await Task.WhenAll(usersTask, txTask, adminTask);
            }
            catch (Exception)
            {
                // Failed to fetch additional data for search results
            }

            var users = usersTask.Result.IsSuccessful && usersTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<UserProfileDto>>(usersTask.Result.Content) ?? new List<UserProfileDto>()
                : new List<UserProfileDto>();

            var transactions = txTask.Result.IsSuccessful && txTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<TransactionDto>>(txTask.Result.Content) ?? new List<TransactionDto>()
                : new List<TransactionDto>();

            // Get admin info
            UserProfileDto? admin = null;
            if (adminTask.Result.IsSuccessful && adminTask.Result.Content != null)
            {
                admin = JsonConvert.DeserializeObject<UserProfileDto>(adminTask.Result.Content);
            }
            else
            {
                admin = users.FirstOrDefault(u => u.Username.Contains("admin", StringComparison.OrdinalIgnoreCase));
            }

            var model = new AdminDashboardViewModel
            {
                Users = users,
                Transactions = transactions,
                Accounts = searchResults,
                Admin = admin
            };

            return PartialView("AdminDashboardView", model);
        }

        // Search transactions by transaction ID, account number, or type
        [HttpPost]
        public async Task<IActionResult> SearchTransactions([FromBody] SearchTransactionsRequest req)
        {
            if (req == null || (!req.TransactionId.HasValue && !req.AccountNumber.HasValue && string.IsNullOrWhiteSpace(req.Type)))
            {
                TempData["Error"] = "Please provide transaction ID, account number, or transaction type to search";
                return await GetView();
            }

            // Check if multiple search criteria provided
            int searchCriteriaCount = 0;
            if (req.TransactionId.HasValue && req.TransactionId > 0) searchCriteriaCount++;
            if (req.AccountNumber.HasValue && req.AccountNumber > 0) searchCriteriaCount++;
            if (!string.IsNullOrWhiteSpace(req.Type)) searchCriteriaCount++;

            if (searchCriteriaCount > 1)
            {
                TempData["Error"] = "Please search by only one criteria at a time";
                return await GetView();
            }

            List<TransactionDto> searchResults = new();

            try
            {
                if (req.TransactionId.HasValue && req.TransactionId > 0)
                {
                    // Search by transaction ID
                    var request = new RestRequest($"/api/transaction/{req.TransactionId}");
                    var response = await _client.ExecuteAsync(request);
                    
                    if (response.IsSuccessful && response.Content != null)
                    {
                        var transaction = JsonConvert.DeserializeObject<TransactionDto>(response.Content);
                        if (transaction != null)
                        {
                            searchResults.Add(transaction);
                            TempData["Message"] = $"Found transaction: {transaction.TransactionId}";
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"No transaction found with ID {req.TransactionId}";
                    }
                }
                else
                {
                    // Search by account number or type - get all transactions and filter
                    var allTransactionsRequest = new RestRequest("/api/transaction/all");
                    var allTransactionsResponse = await _client.ExecuteAsync(allTransactionsRequest);
                    
                    if (allTransactionsResponse.IsSuccessful && allTransactionsResponse.Content != null)
                    {
                        var allTransactions = JsonConvert.DeserializeObject<List<TransactionDto>>(allTransactionsResponse.Content);
                        if (allTransactions != null)
                        {
                            if (req.AccountNumber.HasValue && req.AccountNumber > 0)
                            {
                                searchResults = allTransactions.Where(t => t.AccountNumber == req.AccountNumber).ToList();
                                if (searchResults.Any())
                                {
                                    TempData["Message"] = $"Found {searchResults.Count} transaction(s) for account: {req.AccountNumber}";
                                }
                                else
                                {
                                    TempData["Error"] = $"No transactions found for account: {req.AccountNumber}";
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(req.Type))
                            {
                                searchResults = allTransactions.Where(t => t.Type.Equals(req.Type, StringComparison.OrdinalIgnoreCase)).ToList();
                                if (searchResults.Any())
                                {
                                    TempData["Message"] = $"Found {searchResults.Count} {req.Type} transaction(s)";
                                }
                                else
                                {
                                    TempData["Error"] = $"No {req.Type} transactions found";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Search error: {ex.Message}";
            }

            // Get other data for the dashboard
            var usersRequest = new RestRequest("/api/userprofile/all");
            var accountsRequest = new RestRequest("/api/account/all");
            var adminRequest = new RestRequest("/api/userprofile/by-username/BankAdmin");

            var usersTask = _client.ExecuteAsync(usersRequest);
            var accountsTask = _client.ExecuteAsync(accountsRequest);
            var adminTask = _client.ExecuteAsync(adminRequest);

            try
            {
                await Task.WhenAll(usersTask, accountsTask, adminTask);
            }
            catch (Exception)
            {
                // Failed to fetch additional data for search results
            }

            var users = usersTask.Result.IsSuccessful && usersTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<UserProfileDto>>(usersTask.Result.Content) ?? new List<UserProfileDto>()
                : new List<UserProfileDto>();

            var accounts = accountsTask.Result.IsSuccessful && accountsTask.Result.Content != null
                ? JsonConvert.DeserializeObject<List<AccountDto>>(accountsTask.Result.Content) ?? new List<AccountDto>()
                : new List<AccountDto>();

            // Get admin info
            UserProfileDto? admin = null;
            if (adminTask.Result.IsSuccessful && adminTask.Result.Content != null)
            {
                admin = JsonConvert.DeserializeObject<UserProfileDto>(adminTask.Result.Content);
            }
            else
            {
                admin = users.FirstOrDefault(u => u.Username.Contains("admin", StringComparison.OrdinalIgnoreCase));
            }

            var model = new AdminDashboardViewModel
            {
                Users = users,
                Transactions = searchResults,
                Accounts = accounts,
                Admin = admin
            };

            return PartialView("AdminDashboardView", model);
        }
    }
}

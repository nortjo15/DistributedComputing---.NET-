using BankWebApp.Models;
using BankWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace BankWebApp.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly RestClient client = new RestClient("http://localhost:5142");

        [HttpGet("roleselectionview")]
        public IActionResult GetRoleSelectionView()
        {
            return PartialView("LoginRoleSelectionView");
        }

        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            return PartialView("LoginDefaultView");
        }

        // GET: api/login/authview/{role}
        [HttpGet("authview/{role}")]
        public async Task<IActionResult> GetLoginAuthenticatedView(string role)
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    if (role == "admin")
                    {
                        var adminModel = await GetAdminDashboardModel();
                        return PartialView("~/Views/Admin/AdminDashboardView.cshtml", adminModel);
                    }
                    else if(role == "user")
                    {
                        var username = Request.Cookies["Username"];
                        var userModel = await GetUserDashboardModel(username);
                        return PartialView("~/Views/User/UserDashboardView.cshtml", userModel);
                    }
                    else
                    {
                        return PartialView("LoginErrorView");
                    }
                }
            }

            return PartialView("LoginDefaultView");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("LoginErrorView");
        }

        // Helper method to get Admin Dashboard Model using RestSharp
        private async Task<AdminDashboardViewModel> GetAdminDashboardModel()
        {
            try
            {
                var usersRequest = new RestRequest("/api/userprofile/all");
                var txRequest = new RestRequest("/api/transaction/all");
                var accountsRequest = new RestRequest("/api/account/all");
                var adminRequest = new RestRequest("/api/userprofile/by-username/BankAdmin");

                var usersResponse = await client.ExecuteAsync(usersRequest);
                var txResponse = await client.ExecuteAsync(txRequest);
                var accountsResponse = await client.ExecuteAsync(accountsRequest);
                var adminResponse = await client.ExecuteAsync(adminRequest);

                var users = usersResponse.IsSuccessful && usersResponse.Content != null
                    ? JsonConvert.DeserializeObject<List<UserProfileDto>>(usersResponse.Content) ?? new List<UserProfileDto>()
                    : new List<UserProfileDto>();

                var transactions = txResponse.IsSuccessful && txResponse.Content != null
                    ? JsonConvert.DeserializeObject<List<TransactionDto>>(txResponse.Content) ?? new List<TransactionDto>()
                    : new List<TransactionDto>();

                var accounts = accountsResponse.IsSuccessful && accountsResponse.Content != null
                    ? JsonConvert.DeserializeObject<List<AccountDto>>(accountsResponse.Content) ?? new List<AccountDto>()
                    : new List<AccountDto>();

                var admin = adminResponse.IsSuccessful && adminResponse.Content != null
                    ? JsonConvert.DeserializeObject<UserProfileDto>(adminResponse.Content)
                    : null;

                return new AdminDashboardViewModel
                {
                    Users = users,
                    Transactions = transactions,
                    Accounts = accounts,
                    Admin = admin
                };
            }
            catch (Exception)
            {
                return new AdminDashboardViewModel
                {
                    Users = new List<UserProfileDto>(),
                    Transactions = new List<TransactionDto>(),
                    Accounts = new List<AccountDto>(),
                    Admin = null
                };
            }
        }

        // Helper method to get User Dashboard Model using RestSharp
        private async Task<UserDashboardViewModel> GetUserDashboardModel(string username)
        {
            try
            {
                var profileRequest = new RestRequest($"/api/UserProfile/by-username/{username}");
                var accountsRequest = new RestRequest("/api/Account/all");
                var transactionsRequest = new RestRequest("/api/Transaction/all");
                var profilesRequest = new RestRequest("/api/UserProfile/all");

                var profileResponse = await client.ExecuteAsync(profileRequest);
                var accountsResponse = await client.ExecuteAsync(accountsRequest);
                var transactionsResponse = await client.ExecuteAsync(transactionsRequest);
                var profilesResponse = await client.ExecuteAsync(profilesRequest);

                var profile = profileResponse.IsSuccessful && profileResponse.Content != null
                    ? JsonConvert.DeserializeObject<UserProfileDto>(profileResponse.Content)
                    : null;

                var accounts = accountsResponse.IsSuccessful && accountsResponse.Content != null
                    ? JsonConvert.DeserializeObject<List<AccountDto>>(accountsResponse.Content) ?? new List<AccountDto>()
                    : new List<AccountDto>();

                var transactions = transactionsResponse.IsSuccessful && transactionsResponse.Content != null
                    ? JsonConvert.DeserializeObject<List<TransactionDto>>(transactionsResponse.Content) ?? new List<TransactionDto>()
                    : new List<TransactionDto>();

                var profiles = profilesResponse.IsSuccessful && profilesResponse.Content != null
                    ? JsonConvert.DeserializeObject<List<UserProfileDto>>(profilesResponse.Content) ?? new List<UserProfileDto>()
                    : new List<UserProfileDto>();

                return new UserDashboardViewModel
                {
                    Profile = profile,
                    Accounts = accounts,
                    Transactions = transactions,
                    Profiles = profiles
                };
            }
            catch (Exception)
            {
                return new UserDashboardViewModel
                {
                    Profile = null,
                    Accounts = new List<AccountDto>(),
                    Transactions = new List<TransactionDto>(),
                    Profiles = new List<UserProfileDto>()
                };
            }
        }

        // Fetch User Profile based on username then validate inputted password
        // POST: api/login/auth
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin user)
        {
            var response = new { login = false, role = user.Role };

            try
            {
                // hard coded check - keep this for testing
                if (user.Username.Equals("abc") && user.Password.Equals("123"))
                {
                    Response.Cookies.Append("SessionID", "1234567", new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    Response.Cookies.Append("Username", user.Username, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    response = new { login = true, role = user.Role };
                    return Json(response);
                }

                // Check against actual database
                RestRequest request = new RestRequest($"/api/userprofile/by-username/{user.Username}");
                RestResponse apiResponse = await client.ExecuteAsync(request);

                if (apiResponse.IsSuccessful && apiResponse.Content != null)
                {
                    UserProfile? profile = JsonConvert.DeserializeObject<UserProfile>(apiResponse.Content);
                    
                    if (profile != null && user.Username.Equals(profile.Username) && user.Password.Equals(profile.Password))
                    {
                        Response.Cookies.Append("SessionID", "1234567", new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });
                        Response.Cookies.Append("Username", user.Username, new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });
                        response = new { login = true, role = user.Role };
                        return Json(response);
                    }
                }
            } 
            catch (Exception)
            {
                // Silent fail - return false response
            }

            return Json(response);
        }
    }
}

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
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("LoginRoleSelectionView");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginRoleSelectionView");
        }

        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("LoginDefaultView");
                }
            }
            // Return the partial view as HTML
            return PartialView("LoginDefaultView");
        }

        // GET: api/login/authview/{role}
        [HttpGet("authview/{role}")]
        public IActionResult GetLoginAuthenticatedView(string role)
        {
            Console.WriteLine("IN AUTHVIEW");
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    Console.WriteLine("COOKIE VALUE:", cookieValue, role);
                    if (role == "admin")
                    {
                        return PartialView("~/Views/Admin/AdminDashboardView.cshtml");
                    }
                    else if(role == "user")
                    {
                        return PartialView("~/Views/User/UserDashboardView.cshtml");
                    }
                    else
                    {
                        Console.WriteLine("ERROR VIEW");
                        return PartialView("LoginErrorView");
                    }
                }
            }

            Console.WriteLine("DEFAULT VIEW");
            // Return the partial view as HTML
            return PartialView("LoginDefaultView");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("LoginErrorView");
        }

        // Fetch User Profile based on username then validate inputted password
        // POST: api/login/auth
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin user)
        {
            // Return the partial view as HTML
            var response = new { login = false, role = user.Role };

            var username = user.Username;
            // Get entry based on username
            try
            {
                // hard coded check todo remove
                if (user.Username.Equals("abc") && user.Password.Equals("123"))
                {
                    Response.Cookies.Append("SessionID", "1234567");
                    response = new { login = true, role = user.Role };
                }

                RestRequest request = new RestRequest($"/api/userprofile/by-username/{username}");
                RestResponse apiResponse = await client.ExecuteAsync(request);

                if(apiResponse.IsSuccessful)
                {
                    UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(apiResponse.Content);
                    if(user.Role == "admin")
                    {
                        // Check if user is admin TODO
                    }

                    if (user != null && user.Username.Equals(profile.Username) && user.Password.Equals(profile.Password))
                    {
                        Response.Cookies.Append("SessionID", "1234567");
                        response = new { login = true, role = user.Role };
                    }
                }

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    object error = JsonConvert.DeserializeObject<object>(apiResponse.Content);
                    Console.WriteLine(BadRequest(error));
                    Json(response);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Json(response);
            }

            return Json(response);
        }
    }
}

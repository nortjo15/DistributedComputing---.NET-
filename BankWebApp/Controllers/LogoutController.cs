using Microsoft.AspNetCore.Mvc;

namespace BankWebApp.Controllers
{
    [Route("api/[controller]")]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                Response.Cookies.Delete("SessionID");
            }
            if (Request.Cookies.ContainsKey("Username"))
            {
                Response.Cookies.Delete("Username");
            }

            return PartialView("LogoutView");
        }
    }
}

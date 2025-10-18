using Microsoft.AspNetCore.Mvc;

namespace BankWebApp.Controllers
{
    [Route("api/[controller]")]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            Response.Cookies.Delete("SessionID");
            return PartialView("LogoutView");
        }
    }
}

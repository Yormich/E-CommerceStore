using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;

namespace E_CommerceStore.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        [HttpGet("Login")]
        public ViewResult Login()
        {
            return View("LoginPage");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace E_CommerceStore.Controllers
{
    [Route("")]
    public class MainController : Controller
    {
        [HttpGet("")]
        [HttpGet("/Products")]
        public ViewResult Index()
        {
            return View();
        }
    }
}

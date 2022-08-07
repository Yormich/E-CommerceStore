using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;

namespace E_CommerceStore.Controllers
{
    [Route("")]
    public class MainController : Controller
    {
        [HttpGet("")]
        [HttpGet("/Products")]
        public async Task<ViewResult> Index([FromServices] EStoreContext db)
        {
            await EStoreSeed.PlantSeed(db);
            return View();
        }

        [HttpGet("About")]
        public ViewResult About()
        {
            return View();
        }
    }
}

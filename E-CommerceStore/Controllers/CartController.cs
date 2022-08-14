using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models.DatabaseModels;
using E_CommerceStore.Models.ViewModels;

namespace E_CommerceStore.Controllers
{
    public class CartController : Controller
    {
        [Authorize]
        [HttpGet("Cart")]
        public IActionResult CartPage()
        {
            return View("CartIndex",10);
        }
    }
}

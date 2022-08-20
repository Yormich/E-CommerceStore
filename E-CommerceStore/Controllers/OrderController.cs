using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using E_CommerceStore.Models.DatabaseModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace E_CommerceStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly EStoreContext db;

        public OrderController(EStoreContext db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Seller, Buyer")]
        [HttpGet("user-Account/OrderHistory/{userId:int}")]
        public async Task<IActionResult> Index(int userId)
        {
            var PersonalUserOrders = await db.UserOrders.Where(uo => uo.UserId == userId)
                .ToListAsync();
            
            List<Order> Orders = await db.Orders.Where(o=>
                (PersonalUserOrders.Any(uo=>uo.OrderId == o.Id) == true))
                .ToListAsync();
            return View(Orders);
        }
    }
}

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

        [Authorize(Roles = "Buyer")]
        [HttpGet("user-Account/OrderHistory/{userId:int}")]
        public async Task<IActionResult> Index(int userId)
        {
            User user = await db.Users.Where(user => user.Id == userId)
                .Include(user => user.Orders)
                .ThenInclude(order=>order.Item).FirstAsync();

            return View(user.Orders);
        }

        [HttpGet("user-Account/OrderHistory/User{userId:int}/{itemId:int}")]
        public async Task<IActionResult> ConfirmItemOrder(int userId,int itemId)
        {

            return RedirectToAction("CartPage", "Cart");
        }

        [HttpGet("MakeOrders/{userId:int}")]
        public async Task<IActionResult> ConfirmFromCart(int userId)
        {
            var items = db.Carts.Where(cart => cart.Id == userId)
                .Include(cart => cart.Items)
                .FirstAsync().Result.Items;
           //add orders
            List<Order> orders = new List<Order>();
            List<int> itemsIds = new List<int>();
            items.ForEach(i => itemsIds.Add(i.Id));
            foreach(int id in itemsIds)
            {
                orders.Add(new Order(id, DateTime.Now));
            }
            await db.Orders.AddRangeAsync(orders);
            await db.SaveChangesAsync();

            var createdOrders = db.Orders.Where(o => itemsIds.Contains(o.ItemId));
  
            //add associative table data
            List<UserOrder> userOrders = new List<UserOrder>();
            foreach(Order order in createdOrders)
            {
                userOrders.Add(new UserOrder(userId, order.Id));
                userOrders.Add(new UserOrder(order.Item.SellerId, order.Id));
            }
            Console.WriteLine("Adding UserOrders");
            await db.UserOrders.AddRangeAsync(userOrders);
            await db.SaveChangesAsync();

            //remove added items from cart
            Console.WriteLine("deletind itemcart entries");
            var ItemsInCart = db.itemCarts.Where(ic => ic.CartId == userId);
            db.itemCarts.RemoveRange(ItemsInCart);
            await db.SaveChangesAsync();
            return RedirectToAction("CartPage","Cart");
        }
    }
}

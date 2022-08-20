using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models.DatabaseModels;
using E_CommerceStore.Models.ViewModels;

namespace E_CommerceStore.Controllers
{
    public class CartController : Controller
    {
        private readonly EStoreContext db;

        public CartController(EStoreContext db)
        {
            this.db = db;
        }

        [Authorize]
        [HttpGet("Cart")]
        public async Task<IActionResult> CartPage()
        {
            Cart cart = await GetCartWithItems(GetUserClaimId());

            return View("CartIndex",cart.Items);
        }


        [HttpGet("Cart/Delete/{itemId:int}")]
        public async Task<IActionResult> DeleteFromCart(int itemId)
        {
            Cart cart = await GetCartWithItems(GetUserClaimId());
            IQueryable<ItemCart> entriesToDelete = db.itemCarts.
                Where(ic => ic.CartId == cart.Id && ic.ItemId == itemId);
            db.itemCarts.RemoveRange(entriesToDelete);
            await db.SaveChangesAsync();
            return RedirectToAction("CartPage","Cart");
        }

        [Authorize]
        [HttpGet("Cart/Add/itemId:int")]
        public async Task<IActionResult> AddItem(int itemId)
        {
            int OwnerId = GetUserClaimId();
            Cart cart = await db.Carts.Where(cart => cart.Id == OwnerId)
                .FirstAsync();
            await db.itemCarts.AddAsync(new ItemCart(itemId, cart.Id));
            await db.SaveChangesAsync();
            Item item = await db.Items
                .Include(item => item.ItemType)
                .ThenInclude(type => type.itemPropertyCategories)
                .Include(item => item.PersonalProperties)
                .FirstAsync(item => item.Id == itemId);
          
            return View("ProductPage",item);
        }

        private async Task<Cart> GetCartWithItems(int OwnerId)
        {
            Cart cart = await db.Carts.Where(cart => cart.Id == OwnerId)
                            .Include(cart => cart.Items).FirstAsync();
            Console.WriteLine($"Cart ID: {cart.Id}");
            return cart;
        }

        private int GetUserClaimId()
        {
            var identity = User.Identity as ClaimsIdentity;
            Claim? id = identity?.Claims.FirstOrDefault(claim => claim.Type == "Id");
            if (id == null)
                return int.MaxValue;
            Console.WriteLine($"User ID: {id.Value}");
            return Int32.Parse(id.Value);
        }
    }
}

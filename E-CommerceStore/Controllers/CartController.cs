using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models.DatabaseModels;
using E_CommerceStore.Models.ViewModels;
using E_CommerceStore.Utilities;

namespace E_CommerceStore.Controllers
{
    public class CartController : Controller
    {
        private readonly EStoreContext db;
        private readonly IUserClaimsManager claimsManager;

        public CartController(EStoreContext db, IUserClaimsManager claimsManager)
        {
            this.db = db;
            this.claimsManager = claimsManager;
        }

        [Authorize]
        [HttpGet("Cart")]
        public async Task<IActionResult> CartPage()
        {
            int UserId;
            if (!Int32.TryParse(claimsManager.TryGetClaimValue("Id"), out UserId))
                return NotFound();

            Cart cart = await GetCartWithItems(UserId);

            return View("CartIndex",cart.Items);
        }


        [HttpGet("Cart/Delete/{itemId:int}")]
        public async Task<IActionResult> DeleteFromCart(int itemId)
        {
            int UserId;
            if (!Int32.TryParse(claimsManager.TryGetClaimValue("Id"), out UserId))
                return NotFound();

            Cart cart = await GetCartWithItems(UserId);
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
            int OwnerId;
            if (!Int32.TryParse(claimsManager.TryGetClaimValue("Id"), out OwnerId))
                return NotFound();

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
    }
}

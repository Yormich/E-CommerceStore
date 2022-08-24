using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using E_CommerceStore.Utilities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models.DatabaseModels;
using System.Text.RegularExpressions;


namespace E_CommerceStore.Controllers
{
    [Route("ItemManager")]
    public class ItemManagerController : Controller
    {
        private readonly EStoreContext db;
        private readonly IUserClaimsManager claimsManager;

        public ItemManagerController(EStoreContext db, IUserClaimsManager claimsManager)
        {
            this.db = db;
            this.claimsManager = claimsManager;
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("")]
        public IActionResult Index()
        {
            int SellerId;
            if (!Int32.TryParse(claimsManager.TryGetClaimValue("Id"), out SellerId))
                return NotFound();

            var SellerItems = db.Items.Where(i => i.SellerId == SellerId);
            return View("Index",SellerItems);
        }

        [HttpGet("ChangeStatus/{itemId:int}")]
        public async Task<IActionResult> ChangeSalingStatus(int itemId)
        {
            Item item = await db.Items.Where(i => i.Id == itemId).FirstAsync();
            item.IsForSale = !item.IsForSale;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "ItemManager");
        }

        [HttpGet("PutItem/itemId:int")]
        public async Task<IActionResult> ItemPropertiesForm(int itemId)
        {
            Item item = await db.Items.FirstAsync(i => i.Id == itemId);
            return View("ItemForm", item);
        }

        [HttpPost("UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromForm] int Id, [FromForm] string Name,
            [FromForm] decimal Price, [FromForm] int Amount, [FromForm] IFormFile? Image)
        {
            Item itemToUpdate = await db.Items.FirstAsync(i => i.Id == Id);
            if (ModelState.IsValid)
            {
                Item? item = db.Items.FirstOrDefault(i => i.Name == Name);
                if (item != null && item.Id != itemToUpdate.Id)
                {
                    ModelState.AddModelError("Name", "Another product with this name already exists");
                    return View("ItemForm", itemToUpdate);
                }
                itemToUpdate.Name = Name;
                itemToUpdate.Price = Price;
                itemToUpdate.Amount = Amount;
                if(Image!= null)
                {
                    string basePath = @"wwwroot\StaticImages\ProductImages";
                    basePath = Path.Combine(basePath, Image.FileName);
                    Console.WriteLine(basePath);
                    if (!System.IO.File.Exists(basePath))
                    {
                        string pngFormatRegex = ".png$|.jpg$";
                        Regex formatRegex = new Regex(pngFormatRegex);
                        if(!formatRegex.IsMatch(Image.FileName))
                        {
                            ModelState.AddModelError("AccountImageSource", "Wrong file format");
                            return View("ItemForm", itemToUpdate);
                        }
                        using (var fStream = new FileStream(basePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(fStream);
                        }
                    }
                    itemToUpdate.ImageSource = Image.FileName;
                }
            
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index","ItemManager");
        }

        [HttpGet("DeleteConfirm/{itemId:int}")]
        public async Task<IActionResult> DeleteConfirmation(int itemId)
        {
            Item itemToDelete = await db.Items.Where(i => i.Id == itemId).FirstAsync();

            return View("DeleteConfirmation", itemToDelete);
        }

        [HttpPost("DeleteItem/{itemId:int}")]
        public async Task<IActionResult> Delete(int itemId, string Name)
        {
            Item item = await db.Items.Where(i => i.Id == itemId).FirstAsync();
            if(item.Name != Name)
            {
                ModelState.AddModelError("Name", "Wrong product Name");
                return View("DeleteConfirmation", item);
            }

            var Orders = db.Orders.Where(o => o.ItemId == itemId);
            List<int> orderIds = new List<int>();
            await Orders.ForEachAsync(o => orderIds.Add(o.Id));
            var UserOrders = db.UserOrders.Where(uo => orderIds.Contains(uo.OrderId));
            db.UserOrders.RemoveRange(UserOrders);
            db.Orders.RemoveRange(Orders);
            var itemCarts = db.itemCarts.Where(ic => ic.ItemId == itemId);
            db.itemCarts.RemoveRange(itemCarts);
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "ItemManager");
        }

        [HttpGet("AddNewItem/{sellerId:int}")]
        public async Task<IActionResult> AddNewItem(int sellerId)
        {
            return Ok();
        }
    }
}

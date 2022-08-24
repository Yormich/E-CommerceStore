using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using E_CommerceStore.Utilities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models.DatabaseModels;
using System.Text.RegularExpressions;
using E_CommerceStore.Models.ViewModels;


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
        public IActionResult AddNewItem(int sellerId)
        {
            var Brands = db.Brands;
            var Types = db.ItemTypes;
            ItemAddModel model = new ItemAddModel(Types,Brands,sellerId);
            return View("ItemAdd",model);
        }

        [HttpPost("AddNewItem/Confirm/{sellerId:int}")]
        public async Task<IActionResult> ConfirmAdding(Item itemToAdd,
            [FromRoute]int sellerId,[FromForm]IFormFile? image)
        {
            Console.WriteLine($"Item Name: {itemToAdd.Name}; Item Brand Id {itemToAdd.BrandId}; Item Type: Id:" +
                $"{itemToAdd.ItemTypeId}; Item Price: {itemToAdd.Price}");

            var Brands = db.Brands;
            var ItemTypes = db.ItemTypes;
            ItemAddModel model = new ItemAddModel(ItemTypes, Brands, sellerId);
            model.Item = itemToAdd;

            if(ModelState.IsValid)
            {
                Item? possibleItem = await db.Items.FirstOrDefaultAsync(i => i.Name == itemToAdd.Name);

                if (possibleItem != null)
                {
                    ModelState.AddModelError("Item.Name", "Product with current name already exists");
                    return View("ItemAdd", model);
                }

                if (image != null)
               {
                    string filename = image.FileName;
                    Regex formatRegex = new Regex(@"\.jpg$|\.png$");
                    if(!formatRegex.IsMatch(filename))
                    {
                        ModelState.AddModelError("Item.ImageSource", "Wrong file format");
                        return View("ItemAdd", model);
                    }
                    string imagePath = @"wwwroot\StaticImages\ProductImages";
                    imagePath = Path.Combine(imagePath, filename);

                    using (var fStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fStream);
                    }
                    itemToAdd.ImageSource = filename;
               }
                await db.Items.AddAsync(itemToAdd);
                await db.SaveChangesAsync();
            }
            else
            {
                foreach(var item in ModelState)
                {
                    Console.WriteLine(item.Key + ": ");
                    foreach(var error in item.Value.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                model.ErrorMessage = "Please try again";
                return View("ItemAdd", model);
            }

            return RedirectToAction("Index", "ItemManager");
        }
    }
}

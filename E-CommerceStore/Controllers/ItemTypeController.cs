using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Models.DatabaseModels;
using E_CommerceStore.Database;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceStore.Controllers
{
    public class ItemTypeController : Controller
    {
        private readonly EStoreContext db;

        public ItemTypeController(EStoreContext db)
        {
            this.db = db;
        }

        [HttpGet("AddItemType")]
        public IActionResult AddItemType()
        {
            ItemType itemType = new ItemType();
            return View("ItemTypeAdd",itemType);
        }


        [HttpPost("AddItemType/Confirm")]
        public async Task<IActionResult> ConfirmItemTypeAdding([FromForm] ItemType itemType)
        {
            if(ModelState.IsValid)
            {
                ItemType? checkIfNameUnique = await db.ItemTypes
                    .FirstOrDefaultAsync(it => it.Name == itemType.Name);
                if(checkIfNameUnique != null)
                {
                    ModelState.AddModelError("Name", "Product Type with this name already exists");
                    return View("ItemTypeAdd", itemType);
                }
                await db.ItemTypes.AddAsync(itemType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "ItemManager");
            }
            else
            {
                return View("ItemTypeAdd", itemType);
            }
        }
    }
}

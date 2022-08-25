using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Database;
using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.Controllers
{
    public class BrandController : Controller
    {
        private readonly EStoreContext db;

        public BrandController(EStoreContext db)
        {
            this.db = db;
        }

        [HttpGet("AddBrand")]
        public IActionResult AddBrand()
        {
            ItemBrand brand = new ItemBrand();
            return View("AddBrand", brand);
        }

        [HttpPost("AddBrand/Confirm")]
        public async Task<IActionResult> AddBrandConfirm([FromForm] ItemBrand brand,
            [FromForm] int[] itemTypes)
        {
            if(ModelState.IsValid)
            {
                if(!itemTypes.Any())
                {
                    TempData["Error"] = "Please choose at least 1 product type to proceed";
                    return View("AddBrand", brand);
                }
                ItemBrand? checkBrand = await db.Brands.FirstOrDefaultAsync(b => b.Name == brand.Name);
                if(checkBrand != null)
                {
                    ModelState.AddModelError("Name", "Brand with current name already exists");
                    return View("AddBrand", brand);
                }
                await db.Brands.AddAsync(brand);
                await db.SaveChangesAsync();
                var addedBrand = await db.Brands.FirstAsync(b => b.Name == brand.Name);
                foreach(int itemTypeId in itemTypes)
                {
                    await db.AddAsync(new BrandsTypes(addedBrand.Id,itemTypeId));
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "ItemManager");
            }

            return View("AddBrand",brand);
        }

    }
}

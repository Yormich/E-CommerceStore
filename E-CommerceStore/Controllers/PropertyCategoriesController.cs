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
    [Route("ItemProperties")]
    public class PropertyCategoriesController : Controller
    {
        private readonly EStoreContext db;

        public PropertyCategoriesController(EStoreContext db)
        {
            this.db = db;
        }


        [HttpGet("{itemId:int}")]
        public async Task<IActionResult> ItemProperties(int itemId)
        {
            Item? item = await db.Items.FirstOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
                return NotFound();

            var propertyCategories = await db.PropertyCategories.Where(c => c.ItemTypeId == item.ItemTypeId)
                .Include(c => c.CategoryProperties).ToListAsync();
            foreach (ItemPropertyCategory category in propertyCategories)
            {
                category.CategoryProperties = category.CategoryProperties.Where(p => p.ItemId == itemId)
                    .ToList();
            }
            PropertyCategoryModel model = new PropertyCategoryModel(item, propertyCategories);

            return View("ItemProperties", model);
        }

        [HttpGet("AddCategory/{itemId:int}/{typeId:int}")]
        public IActionResult AddNewCategory(int itemId, int typeId)
        {
            ViewBag.itemId = itemId;
            ItemPropertyCategory category = new ItemPropertyCategory(typeId);
            return View("AddCategory", category);
        }


        [HttpPost("AddCategory/Confirm/{itemId:int}")]
        public async Task<IActionResult> ConfirmCategoryAdd([FromForm] ItemPropertyCategory category,
            int itemId)
        {
            if (ModelState.IsValid)
            {
                ItemPropertyCategory? checkCategory = await db.PropertyCategories.
                    FirstOrDefaultAsync(pc => pc.Name.ToLower().Trim() == category.Name.ToLower().Trim() &&
                    pc.ItemTypeId == category.ItemTypeId);
                if (checkCategory != null)
                {
                    ModelState.AddModelError("Name", "Category with this name already exists");
                    return View("AddCategory", category);
                }
                await db.PropertyCategories.AddAsync(category);
                await db.SaveChangesAsync();
                return RedirectToAction("ItemProperties", "PropertyCategories", new { itemId = itemId });
            }
            return View("AddCategory", category);
        }

        [HttpGet("DeleteCategory/{itemId:int}/{categoryId:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int itemId, [FromRoute] int categoryId)
        {
            ItemPropertyCategory? category = await db.PropertyCategories.Where(c => c.Id == categoryId)
                .Include(c => c.CategoryProperties).FirstOrDefaultAsync();
            if (category == null)
                return NotFound();
            db.RemoveRange(category.CategoryProperties);
            db.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("ItemProperties", "PropertyCategories", new { itemId = itemId });
        }

        [HttpGet("EditCategory/{itemId:int}/{categoryId:int}")]
        public async Task<IActionResult> EditCategoryForm([FromRoute] int itemId, [FromRoute] int categoryId)
        {
            ItemPropertyCategory? category = await db.PropertyCategories
                .FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
                return NotFound();
            ViewBag.itemId = itemId;

            return View("EditCategory", category);
        }

        [HttpPost("EditCategory/Confirm/{itemId:int}")]
        public async Task<IActionResult> EditCategory([FromForm] ItemPropertyCategory category,
            [FromRoute] int itemId)
        {
            ItemPropertyCategory? cat = await db.PropertyCategories
                .FirstOrDefaultAsync(c => c.Id == category.Id);
            if (cat == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                ItemPropertyCategory? checkCategory = await db.PropertyCategories
                    .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim() &&
                    c.ItemTypeId == category.ItemTypeId);
                if (checkCategory != null)
                {
                    ModelState.AddModelError("Name", "Category with this " +
                        "name already exists for this product type");
                    return View("EditCategory", category);
                }
                cat.Name = category.Name;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("ItemProperties", "PropertyCategories", new { itemId = itemId });
        }

        [HttpGet("EditCategoryProperty/{itemId:int}/{propertyId:int}")]
        public async Task<IActionResult> EditPropertyForm([FromRoute] int itemId, [FromRoute] int propertyId)
        {
            ItemProperty? property = await db.ItemProperties.FirstOrDefaultAsync(p => p.Id == propertyId);
            if (property == null)
                return NotFound();
            ViewBag.itemId = itemId;
            return View("EditProperty", property);
        }
        [HttpPost("EditCategoryProperty/{itemId:int}")]
        public async Task<IActionResult> EditProperty([FromForm] ItemProperty property)
        {
            if(ModelState.IsValid)
            {
                ItemProperty? propertyToEdit = await db.ItemProperties
                    .FirstOrDefaultAsync(p=> p.Id == property.Id);
                if (propertyToEdit == null)
                    return NotFound();

                ItemProperty? checkProperty = await db.ItemProperties
                    .FirstOrDefaultAsync(p => p.PropertyName.ToLower().Trim() ==
                    property.PropertyName.ToLower().Trim() && 
                    p.ItemPropertyCategoryId == property.ItemPropertyCategoryId);
                if(checkProperty != null && checkProperty.Id != property.Id)
                {
                    ModelState.AddModelError("PropertyName", "Property with current name" +
                        " already exists in this category");
                    return View("EditProperty", property);
                }
                propertyToEdit.PropertyName = property.PropertyName;
                propertyToEdit.PropertyValue = property.PropertyValue;
                await db.SaveChangesAsync();
                return RedirectToAction("ItemProperties", "PropertyCategories", new { itemId = property.ItemId});
            }
            return View("EditProperty", property);
        }

        [HttpGet("DeleteCategoryProperty/{itemId:int}/{propertyId:int}")]
        public async Task<IActionResult> DeleteProperty([FromRoute] int itemId,[FromRoute] int propertyId)
        {
            ItemProperty? property = await db.ItemProperties.FirstOrDefaultAsync(p => p.Id == propertyId);
            if (property == null)
                return NotFound();
            db.ItemProperties.Remove(property);
            await db.SaveChangesAsync();
            return RedirectToAction("ItemProperties", "PropertyCategories", new { itemId = itemId });
        }


        [HttpGet("AddCategoryProperty/{itemId:int}/{categoryId:int}")]
        public IActionResult AddProperty([FromRoute] int itemId, [FromRoute] int categoryId)
        {
            ItemProperty property = new ItemProperty(itemId,categoryId);
            return View("AddProperty", property);
        }

        [HttpPost("AddCategoryProperty/Confirm")]
        public async Task<IActionResult> AddPropertyConfirm([FromForm] ItemProperty property)
        {
            if(ModelState.IsValid)
            {
                string name = property.PropertyName.ToLower().Trim();
                ItemProperty? checkProperty = await db.ItemProperties
                    .FirstOrDefaultAsync(p => p.PropertyName.ToLower().Trim() == name);
                if(checkProperty!=null)
                {
                    ModelState.AddModelError("PropertyName", "Property with this name already exists");
                    return View("AddProperty", property);
                }
                await db.ItemProperties.AddAsync(property);
                await db.SaveChangesAsync();
                return RedirectToAction("ItemProperties", "PropertyCategories", new { itemId = property.ItemId });
            }
            return View("AddProperty", property);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models.DatabaseModels;
using E_CommerceStore.Models.ViewModels;

namespace E_CommerceStore.Controllers
{
   // [Route("")]
    public class ProductCatalogController : Controller
    {
       // [HttpGet("")]
        [HttpGet("Products")]
        public async Task<ViewResult> Index([FromServices] EStoreContext db,
            [FromServices] ProductCatalogModel viewModel)
        {
            await EStoreSeed.PlantSeed(db);

            viewModel.ResultItems = db.Items;
            return View("Index",viewModel);
        }

        [HttpGet("Products/About")]
        public ViewResult About()
        {
            return View();
        }

        [HttpPost("Products/{typeId:int:min(0)}/{brandId?}")]
        public ViewResult PostIndex([FromServices] EStoreContext db,
            [FromServices] ProductCatalogModel viewModel,int typeId, int? brandId)
        {
            if (typeId != 0)
                viewModel.ResultItems = db.Items.Where(item=>item.ItemTypeId == typeId 
                /*&& item.IsInCart()*/);

            if (brandId is not null && typeId != 0)
                viewModel.ResultItems = viewModel.ResultItems
                    .Where(item => item.BrandId == brandId);

            return View("Index",viewModel);
        }

        [HttpGet("ProductPage/{itemId:int:min(1)}")]
        public ViewResult ProductPage(int itemId)
        {

            return View("About");
        }
    }
}

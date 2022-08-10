using E_CommerceStore.Database;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceStore
{
    public static class ServiceExtensions
    {
        public static void AddProjectServices(this IServiceCollection services)
        {

            services.AddDbContext<EStoreContext>(options =>
            options.UseSqlServer(EStoreContext.MakeConnectionString(
                Directory.GetCurrentDirectory(), "appDbConfig.json")));

            var optionsBuilder = new DbContextOptionsBuilder<EStoreContext>();

            ProductCatalogModel implementation;

            EStoreContext db = new EStoreContext(
                optionsBuilder.UseSqlServer(EStoreContext.MakeConnectionString(
                Directory.GetCurrentDirectory(), "appDbConfig.json")).Options);

            implementation = new ProductCatalogModel(db.ItemTypes.Include(type => type.Brands));
    
            services.AddSingleton<ProductCatalogModel>(implementation);
        }
    }
}

using E_CommerceStore.Database;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace E_CommerceStore
{
    public static class ServiceExtensions
    {
        public static void AddProjectServices(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/denied";
                    options.LogoutPath = "/Products";

                });

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

            services.AddScoped<ItemImagePathProvider>();

            services.AddScoped<UserImagePathProvider>();

           // services.AddSingleton<IFileProvider>();
        }
    }
}

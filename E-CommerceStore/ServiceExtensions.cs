using E_CommerceStore.Database;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

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

            implementation = new ProductCatalogModel(db.ItemTypes.Include(type => type.Brands)
                .ThenInclude(brand => brand.BrandItems));

            services.AddSingleton<ProductCatalogModel>(implementation);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ItemImagePathProvider>();

            services.AddScoped<UserImagePathProvider>();

            services.AddTransient<IUserClaimsManager, UserClaimManager>();

            services.AddSingleton<IEmailVerificator, BaseEmailVerificator>((IServiceProvider provider) =>
            {
                return new BaseEmailVerificator(1_800_000);
            });

           // services.AddSingleton<IFileProvider>();
        }
    }
}

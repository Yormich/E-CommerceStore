using E_CommerceStore.Database;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EStoreContext>(options =>
options.UseSqlServer(EStoreContext.MakeConnectionString(
    Directory.GetCurrentDirectory(), "appDbConfig.json")));

builder.Services.AddControllersWithViews();


WebApplication app = builder.Build();
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "main",
    pattern: "{controller=Main}/{action=Index}/{id?}",
    defaults: new {controller = "Main", action = "Index"}
);

app.Run();

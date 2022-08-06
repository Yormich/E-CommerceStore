WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
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

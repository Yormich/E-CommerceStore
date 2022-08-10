using E_CommerceStore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectServices();

builder.Services.AddControllersWithViews();


WebApplication app = builder.Build();
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Map("/", (HttpContext context) =>
{
    context.Response.Redirect("/Products",true);
});


app.MapControllerRoute(
    name: "main",
    pattern: "Products/{action=Index}",
    defaults: new {controller = "ProductCatalog",action="Index"}
);

app.Run();

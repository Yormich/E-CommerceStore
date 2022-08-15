using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using E_CommerceStore.Models.DatabaseModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceStore.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("login")]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("LoginPage");
        }

        [HttpGet("denied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }


        [HttpGet("register")]
        public ViewResult Register()
        {
            return View("RegisterPage",new UserRegisterModel());
        }

        [HttpPost("register")]
        public async Task<IActionResult> AcceptRegisterInfo([FromServices] EStoreContext db,
            UserRegisterModel model)
        {
            if(ModelState.IsValid)
            {
                if (db.Users.Where(user => user.Email == model.Email).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("Email", "Email already Exists");
                    return View("RegisterPage", model);
                }
                Console.WriteLine("Adding User");
                User user = new User(model.Email, model.Password, null, model.Role);
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                Console.WriteLine("Adding cart");
                int cartOwnerId = db.Users.Where(u => u.Email == user.Email).First().Id;
                Cart cart = new Cart(cartOwnerId);
                await db.Carts.AddAsync(cart);
                await db.SaveChangesAsync();
                Console.WriteLine("Registered Successfully!");
                return RedirectToAction("Login","User");
            }

            return View("RegisterPage",model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string email, string password, string ReturnUrl,
            [FromServices] EStoreContext db)
        {
            User? user = db.Users.FirstOrDefault(u=>u.Email == email && u.Password == password);
            if(user!= null)
            {

                List<Claim> claims = new List<Claim>()
                {
                    new Claim("Id",user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim("Password",user.Password),
                    new Claim(ClaimTypes.Role,GetRole(user.Role)),
                    new Claim("ImageSource",user.AccountImageSource ?? "")
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);
                return Redirect(ReturnUrl);
            }
            TempData["Error"] = "Username or Password is invalid";
            return View("LoginPage");
        }

        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {  
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Products");
        }

        [Authorize]
        [Route("user-Account")]
        public async Task<IActionResult> Account()
        {
            return View("UserProfile");
        }


        private string GetRole(Role RoleId)
        {
            return RoleId == 0 ? "Buyer" : "Seller";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using E_CommerceStore.Models.DatabaseModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            return View("RegisterPage", new UserRegisterModel());
        }

        [HttpPost("register")]
        public async Task<IActionResult> AcceptRegisterInfo([FromServices] EStoreContext db,
            UserRegisterModel model)
        {
            if (ModelState.IsValid)
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

                return RedirectToAction("Login", "User");
            }

            return View("RegisterPage", model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string email, string password, string ReturnUrl,
            [FromServices] EStoreContext db)
        {
            User? user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
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
            TempData["Error"] = "Email or Password is invalid";
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
        [HttpGet("user-Account")]
        public async Task<IActionResult> Account([FromServices] EStoreContext db)
        {
            User user = await GetUserByClaimId(db);

            return View("UserProfile", user);
        }

        [Authorize]
        [HttpPut("user-Account")]

        [Authorize]
        [HttpDelete("user-Account")]

        private string GetRole(Role RoleId)
        {
            return RoleId == 0 ? "Buyer" : "Seller";
        }

        [Authorize]
        [HttpPost("/user-Account/pImage")]
        public async Task<IActionResult> UploadImage([FromServices] EStoreContext db, IFormFile file)
        {
            User user = await GetUserByClaimId(db);

            string pngFormatRegex = ".png$|.jpg$";
            int ThreeMegaBytes = 3 * 1024 * 1024;
            Regex regex = new Regex(pngFormatRegex);
            if (!regex.IsMatch(file.FileName))
            {
                ModelState.AddModelError("AccountImageSource", "Wrong file format");
                return View("UserProfile", user);
            }
            else if (file.Length > ThreeMegaBytes)
            {
                ModelState.AddModelError("AccountImageSource", "File size is bigger than 3Mb");
            }
            user.AccountImageSource = file.FileName;
            await db.SaveChangesAsync();

            string filePath = @"wwwroot\StaticImages\UserImages";
            filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            UpdateClaimValues(new Dictionary<string, string>()
            {
                {"ImageSource",$"{file.FileName}" }
            });

            return View("UserProfile", user);
        }

        [Authorize]
        [HttpPost("user-Account/pMainProps")]
        public async Task<IActionResult> PutMainProps([FromServices] EStoreContext db,
            string newEmail, string oldPassword, string newPassword)
        {
            User user = await GetUserByClaimId(db);

            if (ModelState.IsValid)
            {
                Dictionary<string, string> updatedClaims = new Dictionary<string, string>();
                if (newEmail != user.Email)
                {
                    if (db.Users.FirstOrDefault(user => user.Email == newEmail) != null)
                    {
                        ModelState.AddModelError("Email", "This Email already exists");
                        Response.StatusCode = 400;
                        return View("UserProfile", user);
                    }

                    user.Email = newEmail;
                    updatedClaims.Add(ClaimTypes.Email, user.Email);
                }

                Regex regex = new Regex(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,30}");

                if (oldPassword != user.Password)
                {
                    ModelState.AddModelError("Password", "To update your password you have to enter old one");
                    Response.StatusCode = 400;
                    return View("UserProfile", user);
                }
                else if (!regex.IsMatch(newPassword))
                {
                    ModelState.AddModelError("Password", "Wrong Password Format");
                    Response.StatusCode = 400;
                    return View("UserProfile", user);
                }
                user.Password = newPassword;
                await db.SaveChangesAsync();
                updatedClaims.Add("Password", user.Password);
                UpdateClaimValues(updatedClaims);

            }
            return View("UserProfile", user);
        }


        [Authorize]
        [HttpPost("user-Account/AddProps")]
        public async Task<IActionResult> PutAdditionalProps([FromServices] EStoreContext db,
            string Name,Countries country,DateTime dateOfBirth)
        {
            Console.WriteLine($"{Name} {country} {dateOfBirth}");
            User user = await GetUserByClaimId(db);

            if(ModelState.IsValid)
            {
                Regex nameCheck = new Regex(@"[a-zA-Z]");
                if(Name.Length != nameCheck.Matches(Name).Count)
                {
                    ModelState.AddModelError("Name", "Wrong Name Format");
                    return View("UserProfile", user);
                }

                user.Name = Name;
                if (country != Countries.None && country != user.country)
                {
                    user.country = country;
                }

                DateTime now = DateTime.Now;
                if (dateOfBirth < now && (now.Subtract(dateOfBirth).TotalDays / 365) < 110)
                {
                    user.DateOfBirth = dateOfBirth;
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Wrong DateOfBirth");
                }
                await db.SaveChangesAsync();
            }

            return View("UserProfile",user);
        }

        private async Task<User> GetUserByClaimId([FromServices] EStoreContext db)
        {
            int UserId = 0;
            foreach (Claim claim in User.Claims)
            {
                if (claim.Type == "Id")
                {
                    UserId = Int32.Parse(claim.Value);
                }
            }

            return await db.Users.FirstAsync(user => user.Id == UserId);
        }

        private void UpdateClaimValues(Dictionary<string,string> NameValue)
        {
            var user = User as ClaimsPrincipal;
            if (user == null)
                return;
               
            var identity = user.Identity as ClaimsIdentity;
            if (identity == null)
                return;
                
            foreach (KeyValuePair<string, string> pair in NameValue)
            {
                var possibleClaim = identity.FindFirst(pair.Key);
                if (possibleClaim != null)
                    identity.RemoveClaim(possibleClaim);

                identity.AddClaim(new Claim(pair.Key, pair.Value));
            }
        }
    }
}

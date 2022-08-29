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
using E_CommerceStore.Utilities;
using System.Net.Mail;
using System.Net;

namespace E_CommerceStore.Controllers
{
    public class UserController : Controller
    {
        private readonly EStoreContext db;
        private readonly IUserClaimsManager claimsManager;
        private readonly IEmailVerificator emailVerificator;
        public UserController(EStoreContext db, IUserClaimsManager claimsManager,IEmailVerificator verificator)
        {
            this.db = db;
            this.claimsManager = claimsManager;
            this.emailVerificator = verificator;
        }



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
        public async Task<IActionResult> AcceptRegisterInfo(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Where(user => user.Email == model.Email).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("Email", "Email already Exists");
                    return View("RegisterPage", model);
                }
                emailVerificator.SetCodeForEmail(model.Email);
<<<<<<< HEAD

                await SendCode(model);

                return View("VerifyRegisterData", model);
=======
                string? code = emailVerificator.GetCodeByEmail(model.Email);
                if(code != null)
                {
                    const string senderAddress = "lanister2028@gmail.com";
                    MailAddress from = new MailAddress(senderAddress, "Yormich");
                    MailAddress to = new MailAddress(model.Email);
                    MailMessage codeMessage = new MailMessage(from, to);
                    codeMessage.Subject = "Your Verification code for E-Commerce Store";
                    codeMessage.Body = $"Code: {code}";
                 
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(from.Address, "xljwerowblyyouua");
                    await smtp.SendMailAsync(codeMessage);
          
                    return View("VerifyRegisterData", model);
                }
>>>>>>> 5cbfe45fd54b6dac131662486e278b08cd69cc30
            }

            return View("RegisterPage", model);
        }

        [HttpGet("register/SendCode")]
        public async Task<IActionResult> SendCode(UserRegisterModel model)
        {
            Console.WriteLine(model.Email + model.Password + model.RepeatedPassword + model.VerificationCode + model.Role);
            string? code = emailVerificator.GetCodeByEmail(model.Email);
            if (code != null)
            {
                const string senderAddress = "lanister2028@gmail.com";
                MailAddress from = new MailAddress(senderAddress, "Yormich");
                MailAddress to = new MailAddress(model.Email);
                MailMessage codeMessage = new MailMessage(from, to);
                codeMessage.Subject = "Your Verification code for E-Commerce Store";
                codeMessage.Body = $"Code: {code}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(from.Address, "xljwerowblyyouua");
                await smtp.SendMailAsync(codeMessage);
            }
            return View("VerifyRegisterData", model);
        }


        [HttpPost("register/verify")]
        public async Task<IActionResult> VerifyRegisterData(UserRegisterModel model)
        {
            if(ModelState.IsValid)
            {
                string? validCode = emailVerificator.GetCodeByEmail(model.Email);
                if (validCode == null)
                {
                    ModelState.AddModelError("VerificationCode", "Code expired, please enter a new one");
                    return View("VerifyRegisterData", model);
                }
                else if (validCode != model.VerificationCode)
                {
                    ModelState.AddModelError("VerificationCode", "Wrong code, please try again");
                    return View("VerifyRegisterData", model);
                }
                emailVerificator.EraseCode(model.Email);
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
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string email, string password, string ReturnUrl)
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
        public async Task<IActionResult> Account()
        {
            User user = await GetUserByClaimId();

            return View("UserProfile", user);
        }

        [Authorize]
        [HttpGet("user-Account/Delete/{id:int}")]
        public IActionResult ConfirmDelete(int id)
        {
            return View("DeleteUser", id);
        }

        [Authorize]
        [HttpPost("user-Account/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id,string Password)
        {
            User user = await db.Users.FirstAsync(user => user.Id == id);
            if(user.Password != Password)
            {
                TempData["Password"] = "Wrong Password";
                return View("DeleteUser", id);
            }
            Cart cart = await db.Carts.FirstAsync(cart => cart.Id == id);
            var ItemCarts = db.itemCarts.Where(ic => ic.CartId == cart.Id);
            var UserOrders = db.UserOrders.Where(uo => uo.UserId == id);
            db.itemCarts.RemoveRange(ItemCarts);
            db.UserOrders.RemoveRange(UserOrders);
            db.Carts.Remove(cart);
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login","User");
        }

        private string GetRole(Role RoleId)
        {
            return RoleId == 0 ? "Buyer" : "Seller";
        }

        [Authorize]
        [HttpPost("/user-Account/pImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            User user = await GetUserByClaimId();

            if (file == null)
            {
                ModelState.AddModelError("AccountImageSource", "File wasnt chosen");
                return View("UserProfile", user);
            }
            string pngFormatRegex = ".png$|.jpg$";
            const int ThreeMegaBytes = 3 * 1024 * 1024;
            Regex regex = new Regex(pngFormatRegex);
            if (!regex.IsMatch(file.FileName))
            {
                ModelState.AddModelError("AccountImageSource", "Wrong file format");
                return View("UserProfile", user);
            }
            else if (file.Length > ThreeMegaBytes)
            {
                ModelState.AddModelError("AccountImageSource", "File size is bigger than 3Mb");
                return View("UserProfile", user);
            }
            user.AccountImageSource = file.FileName;
            await db.SaveChangesAsync();

            string filePath = @"wwwroot\StaticImages\UserImages";
            filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            Dictionary<string, string> newClaims = new Dictionary<string, string>()
            {
                {"ImageSource",$"{file.FileName}" }
            };
            foreach(KeyValuePair<string,string> pair in newClaims)
            {
                claimsManager.TryReplaceClaim(pair.Key, pair.Value);
            }
            await HardClaimRewrite(claimsManager.GetClaims());

            return View("UserProfile", user);
        }

        [Authorize]
        [HttpPost("user-Account/pMainProps")]
        public async Task<IActionResult> PutMainProps(string newEmail, string oldPassword, string newPassword)
        {
            User user = await GetUserByClaimId();

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

                if (oldPassword != user.Password)
                {
                    ModelState.AddModelError("Password", "To update your password you have to enter old one");
                    Response.StatusCode = 400;
                    return View("UserProfile", user);
                }

                user.Password = newPassword;
                await db.SaveChangesAsync();
                updatedClaims.Add("Password", user.Password);
                foreach(KeyValuePair<string,string> pair in updatedClaims)
                {
                    claimsManager.TryReplaceClaim(pair.Key, pair.Value);
                }
                await HardClaimRewrite(claimsManager.GetClaims());
            }
            return View("UserProfile", user);
        }


        [Authorize]
        [HttpPost("user-Account/AddProps")]
        public async Task<IActionResult> PutAdditionalProps(
            string Name,Countries country,DateTime dateOfBirth)
        {
            Console.WriteLine($"{Name} {country} {dateOfBirth}");
            User user = await GetUserByClaimId();

            if(ModelState.IsValid)
            {

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
                    return View("UserProfile", user);
                }
                await db.SaveChangesAsync();
            }
            return View("UserProfile",user);
        }
        
        private async Task HardClaimRewrite(IEnumerable<Claim> claims)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsIdentity dentity = new ClaimsIdentity(claims,
                  CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(dentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }

        private async Task<User> GetUserByClaimId()
        {
            int UserId;
            if (!Int32.TryParse(claimsManager.TryGetClaimValue("Id"), out UserId))
                throw new ArgumentNullException("UserId is null");

            User user = await db.Users.FirstAsync(u => u.Id == UserId);
            return user;
        } 
    }
}

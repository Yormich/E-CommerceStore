using E_CommerceStore.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models.ViewModels
{
    public class UserRegisterModel
    {
        [Required]
        public Role Role { get; set; }

        [EmailAddress(ErrorMessage = "Wrong Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,30}",
            ErrorMessage = "Wrong Password Format")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat your Password")]
        [Compare("Password",ErrorMessage ="Passwords don't match")]
        public string RepeatedPassword { get; set; }

        public UserRegisterModel(Role role, string email, string password, string repeatedPassword)
        {
            Role = role;
            Email = email;
            Password = password;
            this.RepeatedPassword = repeatedPassword;
        }

        public UserRegisterModel() { }
    }
}

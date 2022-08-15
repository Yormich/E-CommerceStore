using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models.DatabaseModels
{
    public enum Countries
    {
        [Display(Name = "Ukraine")]
        Ukraine,
        [Display(Name = "England")]
        England,
        [Display(Name = "Poland")]
        Poland,
        [Display(Name = "Germany")]
        Germany,
        [Display(Name = "USA")]
        USA,
        [Display(Name = "France")]
        France
    };

    public enum Role
    {
        [Display(Name ="Buyer")]
        Buyer,
        [Display(Name ="Seller")]
        Seller
    }
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public string? Name { get; set; }

        public Role Role { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime RegisteredSince { get; set; }

        public Countries? country { get; set; }

        public string? AccountImageSource { get; set; }

        public List<Item> Items { get; set; } = null!;

        public Cart? Cart { get; set; } = null!;

        public List<Order> Orders { get; set; } = null!;

        public User(string Email, string Password, string? Name, Role Role)
        {
            this.Email = Email;
            this.Password = Password;
            this.Role = Role;
            this.Name = Name;
        }

        protected User() { }
    }
}

namespace E_CommerceStore.Models
{
    public enum Countries
    {
        Ukraine,
        England,
        Poland,
        Germany,
        USA,
        France
    };

    public enum Role
    {
        Buyer,
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

        public string? AccountImageSource { get; set; } = "";

        public List<Item> Items { get; set; } = null!;

        public Cart? Cart { get; set; } = null!;
        public int CartId { get; set; }

        public List<Order> Orders { get; set; } = null!;

        public User(string Email, string Password,string? Name, Role Role,int CartId)
        {
            this.Email = Email;
            this.Password = Password;
            this.Role = Role;
            this.Name = Name;
            this.CartId = CartId;
        }

        protected User() { }
    }
}

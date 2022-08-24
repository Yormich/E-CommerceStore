using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models.DatabaseModels
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name isn't set")]
        [StringLength(50,ErrorMessage ="Name length should be between 3 and 50 characters",MinimumLength =3)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Set price to proceed")]
        public decimal Price { get; set; }

        public string? ImageSource { get; set; } = string.Empty;

        public ItemBrand? Brand { get; set; }
        public int BrandId { get; set; }

        public ItemType? ItemType { get; set; }
        public int ItemTypeId { get; set; }

        public User? ItemSeller { get; set; }

        public int SellerId { get; set; }

        public List<ItemPropertyCategory> Categories { get; set; } = new List<ItemPropertyCategory>();

        public List<ItemProperty> PersonalProperties { get; set; } = new List<ItemProperty>();

        public List<Cart> Carts { get; set; } = new List<Cart>();

        [Required(ErrorMessage = "Product Amount is required")]
        public int Amount { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public Item(string Name, decimal Price, int BrandId, int TypeId, int SellerId, int Amount = 1)
        {
            this.Name = Name;
            this.Price = Price;
            this.BrandId = BrandId;
            this.ItemTypeId = TypeId;
            this.SellerId = SellerId;
            this.Amount = Amount;
        }

        public bool IsForSale { get; set; }

        public Item() { }
    }
}

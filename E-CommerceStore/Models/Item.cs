namespace E_CommerceStore.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public decimal Price { get; set; }
        public string? ImageSource { get; set; } = String.Empty;

        public ItemBrand Brand { get; set; } = null!;
        public int BrandId { get; set; }

        public ItemType ItemType { get; set; } = null!;
        public int ItemTypeId { get; set; }

        public User ItemSeller { get; set; } = null!;

        public int SellerId { get; set; }

        public List<ItemPropertyCategory> Categories { get; set; } = null!;

        public Cart? Cart { get; set; }

        public int? CartId { get; set; }

        public Order? Order { get; set; }
    }
}

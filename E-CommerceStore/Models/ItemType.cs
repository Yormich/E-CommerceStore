namespace E_CommerceStore.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public List<Item> Items { get; set; } = null!;

        public List<ItemBrand> Brands { get; set; } = null!;
        public List<ItemPropertyCategory> itemPropertyCategories { get; set; } = null!;
    }
}

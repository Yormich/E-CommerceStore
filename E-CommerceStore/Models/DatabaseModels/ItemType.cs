namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Item> Items { get; set; } = null!;

        public List<ItemBrand> Brands { get; set; } = null!;
        public List<ItemPropertyCategory> itemPropertyCategories { get; set; } = null!;
    }
}

namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemBrand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Item> BrandItems { get; set; } = null!;
        public List<ItemType> ItemTypes { get; set; } = null!;
    }
}

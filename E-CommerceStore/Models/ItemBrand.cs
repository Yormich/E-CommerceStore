namespace E_CommerceStore.Models
{
    public class ItemBrand
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public List<Item> BrandItems { get; set; } = null!;
        public List<ItemType> ItemTypes { get; set; } = null!;
    }
}

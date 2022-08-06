namespace E_CommerceStore.Models
{
    public class ItemBrand
    {
        public int Id { get; set; } 
        public string Name { get; set; } = String.Empty;
        public Item Item { get; set; } = null!;
    }
}

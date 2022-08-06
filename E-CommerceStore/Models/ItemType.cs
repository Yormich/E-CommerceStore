namespace E_CommerceStore.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public Item Item { get; set; } = null!;
    }
}

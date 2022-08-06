namespace E_CommerceStore.Models
{
    public class ItemPropertyCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public Item MasterItem { get; set; } = null!;

        public int ItemId { get; set; }

        public List<ItemProperty> CategoryProperties { get; set; } = null!;
    }
}

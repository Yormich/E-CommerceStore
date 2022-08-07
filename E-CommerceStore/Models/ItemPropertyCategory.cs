namespace E_CommerceStore.Models
{
    public class ItemPropertyCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public ItemType ItemType { get; set; } = null!;

        public int ItemTypeId { get; set; }
        public List<ItemProperty> CategoryProperties { get; set; } = null!;

        public ItemPropertyCategory(string name,int itemTypeId)
        {
            this.Name = name;
            this.ItemTypeId = itemTypeId;
        }
    }
}

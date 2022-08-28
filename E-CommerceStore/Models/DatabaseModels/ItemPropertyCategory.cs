using System.ComponentModel.DataAnnotations;


namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemPropertyCategory
    {
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z ]{3,50}",ErrorMessage = "Name must contain letters only with length between 3 and 50 characters")]
        public string Name { get; set; } = String.Empty;

        public ItemType? ItemType { get; set; } 

        public int ItemTypeId { get; set; }
        public List<ItemProperty> CategoryProperties { get; set; } = new List<ItemProperty>();

        //for form Registration
        public ItemPropertyCategory(int itemTypeId)
        {
            this.ItemTypeId = itemTypeId;
        }

        public ItemPropertyCategory(string Name, int itemTypeId)
        {
            this.Name = Name;
            this.ItemTypeId = itemTypeId;
        }

        public ItemPropertyCategory() { }
    }
}

using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemProperty
    {
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z ]{3,50}$",
           ErrorMessage = "Name must contain letters only with length between 3 and 50 characters")]
        public string PropertyName { get; set; } = String.Empty;

        [StringLength(50,ErrorMessage = "Value length should be between 2 and 50 characters",MinimumLength = 2)]
        public string PropertyValue { get; set; } = String.Empty;

        public int ItemPropertyCategoryId { get; set; }
        public ItemPropertyCategory? PropertyCategory { get; set; }

        public Item? Item { get; set; }

        public int ItemId { get; set; }

        public ItemProperty(string PropertyName, string PropertyValue,
            int ItemPropertyCategoryId,int ItemId)
        {
            this.PropertyName = PropertyName;
            this.PropertyValue = PropertyValue;
            this.ItemPropertyCategoryId = ItemPropertyCategoryId;
            this.ItemId = ItemId;
        }

        public ItemProperty(int ItemId, int ItemCategoryId)
        {
            this.ItemId = ItemId;
            this.ItemPropertyCategoryId = ItemCategoryId;
        }

        public ItemProperty() { }
    }
}

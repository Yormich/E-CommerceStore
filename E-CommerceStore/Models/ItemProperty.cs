namespace E_CommerceStore.Models
{
    public class ItemProperty
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = String.Empty;

        public string PropertyValue { get; set; } = String.Empty;

        public int ItemPropertyCategoryId { get; set; }
        public ItemPropertyCategory PropertyCategory { get; set; } = null!;

        public ItemProperty(string PropertyName,string PropertyValue,int ItemPropertyCategoryId)
        {
            this.PropertyName = PropertyName;
            this.PropertyValue = PropertyValue;
            this.ItemPropertyCategoryId = ItemPropertyCategoryId;
        }
    }
}

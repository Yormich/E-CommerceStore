namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemProperty
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;

        public string PropertyValue { get; set; } = string.Empty;

        public int ItemPropertyCategoryId { get; set; }
        public ItemPropertyCategory PropertyCategory { get; set; } = null!;

        public ItemProperty(string PropertyName, string PropertyValue, int ItemPropertyCategoryId)
        {
            this.PropertyName = PropertyName;
            this.PropertyValue = PropertyValue;
            this.ItemPropertyCategoryId = ItemPropertyCategoryId;
        }
    }
}

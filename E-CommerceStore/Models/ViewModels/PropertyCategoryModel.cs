using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.Models.ViewModels
{
    public class PropertyCategoryModel
    {
        public IEnumerable<ItemPropertyCategory> Categories { get; set; }
        public Item OwnerItem { get; set; }

        public PropertyCategoryModel(Item OwnerItem,IEnumerable<ItemPropertyCategory> Categories)
        {
            this.Categories = Categories;
            this.OwnerItem = OwnerItem;
        }
    }
}

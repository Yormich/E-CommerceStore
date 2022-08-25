using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemType
    {
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z ]{3,50}$",
            ErrorMessage = "Name must contain letters only with length between 3 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        public List<Item> Items { get; set; } = new List<Item>();

        public List<ItemBrand> Brands { get; set; } = new List<ItemBrand>();
        public List<ItemPropertyCategory> itemPropertyCategories { get; set; } = new List<ItemPropertyCategory>();

        public ItemType() { }
    }
}

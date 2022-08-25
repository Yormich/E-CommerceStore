using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models.DatabaseModels
{
   
    public class ItemBrand
    {
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z ]{3,50}$",
           ErrorMessage = "Name must contain letters only with length between 3 and 50 characters")]
        public string Name { get; set; } = String.Empty;

        public List<Item> BrandItems { get; set; } = new List<Item>();
        public List<ItemType> ItemTypes { get; set; } = new List<ItemType>();

        public ItemBrand() { }
    }
}

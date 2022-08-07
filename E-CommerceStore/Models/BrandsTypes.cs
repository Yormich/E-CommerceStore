using E_CommerceStore.Models;

namespace E_CommerceStore.Models
{
    public class BrandsTypes
    {
        public int ItemBrandId { get; set; }
        public ItemBrand? Brand { get; set; }

        public int ItemTypeId { get; set; }
        public ItemType? ItemType { get; set; }

        public BrandsTypes(int itemBrandId, int ItemTypeId)
        {
            this.ItemBrandId = itemBrandId;
            this.ItemTypeId = ItemTypeId;
        }
    }
}

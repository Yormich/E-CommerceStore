namespace E_CommerceStore.Models.DatabaseModels
{
    public class BrandsTypes
    {
        public int ItemBrandId { get; set; }
        public ItemBrand? Brand { get; set; }

        public int ItemTypeId { get; set; }
        public ItemType? ItemType { get; set; }

        public BrandsTypes(int itemBrandId, int ItemTypeId)
        {
            ItemBrandId = itemBrandId;
            this.ItemTypeId = ItemTypeId;
        }
    }
}

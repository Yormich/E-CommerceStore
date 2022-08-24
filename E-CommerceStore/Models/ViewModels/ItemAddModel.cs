using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.Models.ViewModels
{
    public class ItemAddModel
    {
        public IEnumerable<ItemType> AvailableTypes { get; set; } = null!;
        public IEnumerable<ItemBrand> AvailableBrands { get; set; } = null!;
        
        public int SellerId { get; set; }

        public Item Item { get; set; }

        public string? ErrorMessage { get; set; }

        public ItemAddModel(IEnumerable<ItemType> availableTypes, 
            IEnumerable<ItemBrand> availableBrands, int sellerId)
        {
            this.AvailableTypes = availableTypes;
            this.AvailableBrands = availableBrands;
            this.SellerId = sellerId;
            this.Item = new Item();
        }
    }
}

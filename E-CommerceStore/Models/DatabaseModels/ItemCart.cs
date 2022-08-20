using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.Models.DatabaseModels
{
    public class ItemCart
    {
        public int ItemId { get; set; }

        public Item? Item { get; set; }

        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public ItemCart(int itemId, int cartId)
        {
            ItemId = itemId;
            CartId = cartId;
        }
    }
}

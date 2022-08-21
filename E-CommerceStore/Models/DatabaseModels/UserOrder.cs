namespace E_CommerceStore.Models.DatabaseModels
{
    public class UserOrder
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public UserOrder(int userId, int orderId)
        {
            UserId = userId;
            OrderId = orderId;
        }
    }
}

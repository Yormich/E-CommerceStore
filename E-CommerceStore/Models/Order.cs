namespace E_CommerceStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Item Item { get; set; } = null!;
        public int ItemId { get; set; }

        //one seller and one buyer
        public List<User> UsersInOrder { get; set; } = null!;
    }
}

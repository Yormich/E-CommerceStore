namespace E_CommerceStore.Models.DatabaseModels
{
    public class Order
    {
        public int Id { get; set; }
        public Item Item { get; set; } = null!;
        public int ItemId { get; set; }

        public DateTime CreatedDate { get; set; }

        //one seller and one buyer
        public List<User> UsersInOrder { get; set; } = null!;

        public Order(int ItemId,DateTime CreatedDate)
        {
            this.ItemId = ItemId;
            this.CreatedDate = CreatedDate;
        }
    }
}

namespace E_CommerceStore.Models.DatabaseModels
{
    public class Cart
    {
        public int Id { get; set; }

        public User Owner { get; set; } = null!;

        public int OwnerId { get; set; }

        public Cart(int OwnerId)
        {
            this.OwnerId = OwnerId;
        }

        public List<Item> Items { get; set; } = null!;
    }
}

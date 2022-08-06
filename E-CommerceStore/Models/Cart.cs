namespace E_CommerceStore.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public User Owner { get; set; } = null!;

        public List<Item> Items { get; set; } = null!;
    }
}

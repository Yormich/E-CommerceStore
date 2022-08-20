﻿namespace E_CommerceStore.Models.DatabaseModels
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string? ImageSource { get; set; } = string.Empty;

        public ItemBrand Brand { get; set; } = null!;
        public int BrandId { get; set; }

        public ItemType ItemType { get; set; } = null!;
        public int ItemTypeId { get; set; }

        public User ItemSeller { get; set; } = null!;

        public int SellerId { get; set; }

        public List<ItemPropertyCategory> Categories { get; set; } = null!;

        public List<ItemProperty> PersonalProperties { get; set; } = null!;

        public List<Cart> Carts { get; set; } = null!;
        public int Amount { get; set; } 

        public Order? Order { get; set; }

        public Item(string Name, decimal Price, int BrandId, int TypeId, int SellerId, int Amount = 1)
        {
            this.Name = Name;
            this.Price = Price;
            this.BrandId = BrandId;
            ItemTypeId = TypeId;
            this.SellerId = SellerId;
            this.Amount = Amount;
        }

        protected Item() { }
    }
}

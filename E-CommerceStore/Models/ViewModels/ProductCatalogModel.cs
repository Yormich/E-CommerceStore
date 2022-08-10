using E_CommerceStore.Models.DatabaseModels;
using System.Collections.Generic;

namespace E_CommerceStore.Models.ViewModels
{
    public class ProductCatalogModel
    {
        public IEnumerable<ItemType> ItemTypes { get; set; }  = Enumerable.Empty<ItemType>();
        public IEnumerable<Item> ResultItems { get; set; } = Enumerable.Empty<Item>();

        public ProductCatalogModel(IEnumerable<ItemType> types)
        {
            ItemTypes = types;
        }

        public ProductCatalogModel() { }
    }
}

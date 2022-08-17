using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.Database
{
    public static class EStoreSeed
    {
        public static async Task PlantSeed(EStoreContext context)
        {
            context.Database.Migrate();
            if(!context.Brands.Any())
            {
                Console.WriteLine("Adding brands");
                await context.Brands.AddRangeAsync(MakeItemBrands());
                await context.SaveChangesAsync();
            }
            if (!context.ItemTypes.Any())
            {
                Console.WriteLine("Adding Item Types");
                await context.ItemTypes.AddRangeAsync(MakeItemTypes());
                await context.SaveChangesAsync();
            }
            if (!context.BrandsTypes.Any())
            {
                Console.WriteLine("Adding associative table data(brands and types)");
                await context.BrandsTypes.AddRangeAsync(MakeBrandsTypes());
                await context.SaveChangesAsync();
            }
            if (!context.Users.Any())
            {
                Console.WriteLine("AddingUsers");
                await context.Users.AddRangeAsync(MakeUsers());
                await context.SaveChangesAsync();
            }
            if (!context.Carts.Any())
            {
                Console.WriteLine("Adding carts");
                await context.Carts.AddRangeAsync(MakeCarts());
                await context.SaveChangesAsync();
            }
            if (!context.Items.Any())
            {
                Console.WriteLine("Adding items");
                await context.Items.AddRangeAsync(MakeItems());
                await context.SaveChangesAsync();
            }
            if(!context.PropertyCategories.Any())
            {
                Console.WriteLine("Adding Categories");
                await context.PropertyCategories.AddRangeAsync(MakeItemPropertyCategories());
                await context.SaveChangesAsync();
            }
            if (!context.ItemProperties.Any())
            {
                Console.WriteLine("Adding Properties");
                await context.ItemProperties.AddRangeAsync(MakeItemProperties());
                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<ItemBrand> MakeItemBrands()
        {
            return new List<ItemBrand>()
            {
                new ItemBrand(){Name="Apple"},
                new ItemBrand(){Name="Mac"},
                new ItemBrand(){Name="Prada"},
                new ItemBrand(){Name="Samsung"}
            };

        }

        private static IEnumerable<ItemType> MakeItemTypes()
        {
            return new List<ItemType>()
            {
                new ItemType(){Name="Smartphone"},
                new ItemType(){Name="Lipstick"},
                new ItemType(){Name="Dress"},
                new ItemType(){Name = "Powder"}
            };
        }

        private static IEnumerable<BrandsTypes> MakeBrandsTypes()
        {
            return new List<BrandsTypes>()
            {
                new BrandsTypes(1,1),
                new BrandsTypes(2,2),
                new BrandsTypes(3,3),
                new BrandsTypes(2,4),
                new BrandsTypes(4,1)
            };
        }

        private static IEnumerable<Item> MakeItems()
        {
            return new List<Item>()
            {
                new Item("iPhone 13 Pro",44999,1,1,1){ImageSource="IPhone13Pro.png"},
                new Item("iPhone 11",21999,1,1,1){ImageSource="IPhone11.png" },
                new Item("Powder Kiss Lipstick",800,2,2,2){ImageSource="MacKissLipstick.png"},
                new Item("LustreGlass Sheer-Shine Lipstick",770,2,2,2){ImageSource="SheerLipstickMac.png" },
                new Item("Cady dress with feathers",4200,3,3,3){ImageSource="CadyDressTest.png" },
                new Item("Embroidered mesh dress",2800,3,3,3){ImageSource="MeshDressTest.png"},
                new Item("Mac Opalescent Powder",582,2,4,3){ImageSource="MacOpalescentPowder.png" },
                new Item("Galaxy S21",29399,4,1,1){ImageSource="Galaxy S21.png" }
            };
        }
        
        private static IEnumerable<ItemPropertyCategory> MakeItemPropertyCategories()
        {
            return new List<ItemPropertyCategory>()
            {
                new ItemPropertyCategory("Main Properties",1),
                new ItemPropertyCategory("Memory",1),
                new ItemPropertyCategory("Size",3),
                new ItemPropertyCategory("Description",2),
                new ItemPropertyCategory("Description",4),
            };
        }

        private static IEnumerable<ItemProperty> MakeItemProperties()
        {
            return new List<ItemProperty>()
            {
                new ItemProperty("Display Diagonal","7.3",1,1),
                new ItemProperty("Screen Type","Super Redina XDR",1,1),
                new ItemProperty("RAM","6 Gb",2,1),
                new ItemProperty("External Memory","256 Gb",2, 1),

                new ItemProperty("Display Diagonal","6.1",1,2),
                new ItemProperty("Screen Type","Super Redina XDR",1,2),
                new ItemProperty("RAM","4 Gb",2,2),
                new ItemProperty("External Memory","128 Gb",2, 2),

                new ItemProperty("Capacity","3 g",4, 3),
                new ItemProperty("Made In","China",4, 3),

                new ItemProperty("Capacity","12 g",4, 4),
                new ItemProperty("Made In","Italy",4, 4),

                new ItemProperty("Dress Size","40",3, 5),

                new ItemProperty("Dress Size","42",3, 6),

                new ItemProperty("Capacity","10 g",5, 7),
                new ItemProperty("Made In","Spain",5, 7),

                new ItemProperty("Display Diagonal","6.4",1,8),
                new ItemProperty("Screen Type","Super Amoled",1,8),
                new ItemProperty("RAM","6 Gb",2,8),
                new ItemProperty("External Memory","256 Gb",2, 8)
            };
        }

        private static IEnumerable<User> MakeUsers()
        {
            return new List<User>()
            {
                new User("bestSeller@gmail.com","Dash212","Saul Goodman",Role.Seller),
                new User("anotherSeller@gmail.com","Qwerty404","Nick Paul",Role.Seller),
                new User("forgotMyEmail@gmail.com","AndPassword2","Billie Kitchen",Role.Seller),
                new User("lanister2028@gmail.com","Dima2028",null,Role.Buyer)
                { AccountImageSource= "Luntik.png"},
                new User("difficultMail99@gmail.com","randomPass",null,Role.Buyer),
                new User("basicMail14@gmail.com","asd12345678",null,Role.Buyer)
            };
        }

        private static IEnumerable<Cart> MakeCarts()
        {
            return new List<Cart>()
            {
                new Cart(1),
                new Cart(2),
                new Cart(3),
                new Cart(4),
                new Cart(5),
                new Cart(6),
            };
        }
    }
}

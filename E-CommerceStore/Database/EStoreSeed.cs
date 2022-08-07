﻿using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models;
using System.Collections.Generic;

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
            if (!context.Carts.Any())
            {
                Console.WriteLine("Adding carts");
                await context.Carts.AddRangeAsync(MakeCarts());
                await context.SaveChangesAsync();
            }
            if (!context.Users.Any())
            {
                Console.WriteLine("AddingUsers");
                await context.Users.AddRangeAsync(MakeUsers());
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
                new ItemBrand(){Name="Prada"}
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
                new BrandsTypes(2,4)
            };
        }

        private static IEnumerable<Item> MakeItems()
        {
            return new List<Item>()
            {
                new Item("iPhone 13 Pro",44999,1,1,1),
                new Item("iPhone 11",21999,1,1,1),
                new Item("Powder Kiss Lipstick",800,2,2,2),
                new Item("LustreGlass Sheer-Shine Lipstick",770,2,2,2),
                new Item("Cady dress with feathers",4200,3,3,3),
                new Item("Embroidered mesh dress",2800,3,3,3),
                new Item("Mac Opalescent Powder",582,2,4,3)
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
                new ItemProperty("Display Diagonal","6.1",1),
                new ItemProperty("Screen Type","Super Redina XDR",1),
                new ItemProperty("RAM","4",2),
                new ItemProperty("External Memory","128 Gb",2),
                new ItemProperty("Dress Size","40 Gb",3),
                new ItemProperty("Capacity","3 g",4),
                new ItemProperty("Made In","China",4),
                new ItemProperty("Capacity","10 g",5),
                new ItemProperty("Made In","Spain",5),

            };
        }

        private static IEnumerable<User> MakeUsers()
        {
            return new List<User>()
            {
                new User("bestSeller@gmail.com","Dash212","Saul Goodman",Role.Seller,1),
                new User("anotherSeller@gmail.com","Qwerty404","Nick Paul",Role.Seller,2),
                new User("forgotMyEmail@gmail.com","AndPassword2","Billie Kitchen",Role.Seller,3),
                new User("lanister2028@gmail.com","Dima2028",null,Role.Buyer,4),
                new User("difficultMail99@gmail.com","randomPass",null,Role.Buyer,5),
                new User("basicMail14@gmail.com","asd12345678",null,Role.Buyer,6)
            };
        }

        private static IEnumerable<Cart> MakeCarts()
        {
            return new List<Cart>()
            {
                new Cart(),
                new Cart(),
                new Cart(),
                new Cart(),
                new Cart(),
                new Cart(),
            };
        }
    }
}
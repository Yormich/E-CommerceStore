using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_CommerceStore.Database
{
    public class EStoreContext : DbContext
    {
        private static string connectionString = String.Empty;

        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                if(connectionString == String.Empty)
                {
                    connectionString = value;
                }
            }
        }

        public DbSet<ItemBrand> Brands { get; set; } = null!;

        public DbSet<ItemType> ItemTypes { get; set; } = null!;

        public DbSet<ItemPropertyCategory> PropertyCategories { get; set; } = null!;

        public DbSet<ItemProperty> ItemProperties { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;


        public static string MakeConnectionString(string ContentRootPath,
            string jsonFileName)
        {
            if(String.IsNullOrEmpty(connectionString))
            {
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(ContentRootPath);
                builder.AddJsonFile(jsonFileName);

                var options = builder.Build();

                ConnectionString = options.GetConnectionString("DefaultConnection");
            }
            return ConnectionString;
        }


        public EStoreContext(DbContextOptions<EStoreContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ItemBrand>(ConfigureItemBrand);
            builder.Entity<ItemType>(ConfigureItemType);
            builder.Entity<ItemPropertyCategory>(ConfigureItemPropertyCategory);
            builder.Entity<ItemProperty>(ConfigureItemProperty);
            builder.Entity<Item>(ConfigureItem);
            builder.Entity<User>(ConfigureUser);
            builder.Entity<Order>(ConfigureOrder);
            builder.Entity<Cart>(ConfigureCart);
        }
         
        private void ConfigureItemBrand(EntityTypeBuilder<ItemBrand> brandBuilder)
        {
            brandBuilder.ToTable("Brands");
            brandBuilder.HasKey(b => b.Id);
            brandBuilder.HasAlternateKey(b => b.Name);
            brandBuilder.Property(b => b.Name).HasMaxLength(50);
            brandBuilder.HasCheckConstraint("Name", "LEN(Name) > 2");
        }
        private void ConfigureItemType(EntityTypeBuilder<ItemType> typeBuilder)
        {
            typeBuilder.ToTable("Types");
            typeBuilder.HasKey(t => t.Id);
            typeBuilder.HasAlternateKey(t => t.Name);
            typeBuilder.Property(t => t.Name).HasMaxLength(50);
            typeBuilder.HasCheckConstraint("Name", "LEN(Name) > 3");
            
        }
        private void ConfigureItemPropertyCategory(EntityTypeBuilder<ItemPropertyCategory> ipBuilder)
        {
            ipBuilder.ToTable("PropertyCategories");
            ipBuilder.HasKey(c => c.Id);
            ipBuilder.HasCheckConstraint("Name", "LEN(Name) > 3");
            ipBuilder.Property(c => c.Name).HasMaxLength(40);

            ipBuilder.HasOne(c => c.MasterItem)
                .WithMany(i => i.Categories)
                .HasForeignKey(c => c.ItemId);
        }

        private void ConfigureItemProperty(EntityTypeBuilder<ItemProperty> propertyBuilder)
        {
            propertyBuilder.ToTable("Properties");
            propertyBuilder.HasKey(p => p.Id);
            propertyBuilder.Property(p => p.PropertyName).HasMaxLength(50);
            propertyBuilder.HasCheckConstraint("PropertyName", "LEN(PropertyName) > 4");
            propertyBuilder.Property(p => p.PropertyValue).HasMaxLength(50);
            propertyBuilder.HasCheckConstraint("PropertyValue", "LEN(PropertyValue) > 0");

            propertyBuilder.HasOne(p => p.PropertyCategory)
                .WithMany(c => c.CategoryProperties)
                .HasForeignKey(p => p.ItemPropertyCategoryId);
        }

        private void ConfigureItem(EntityTypeBuilder<Item> itemsBuilder)
        {
            itemsBuilder.ToTable("Items");
            itemsBuilder.HasKey(i => i.Id);
            itemsBuilder.HasAlternateKey(i => i.Name);
            itemsBuilder.Property(i => i.Name).HasMaxLength(70);
            itemsBuilder.HasCheckConstraint("Name", "LEN(Name) > 5");
            itemsBuilder.HasCheckConstraint("Price", "Price > 0");
            itemsBuilder.Property(i => i.Price).HasColumnType("decimal");

            itemsBuilder.HasOne(i => i.Brand)
                .WithOne(b => b.Item)
                .HasForeignKey<Item>(i => i.BrandId);

            itemsBuilder.HasOne(i => i.ItemType)
                .WithOne(t => t.Item)
                .HasForeignKey<Item>(i => i.ItemTypeId);

            itemsBuilder.HasOne(i => i.ItemSeller)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SellerId);

            itemsBuilder.HasOne(i => i.Cart)
                .WithMany(cart => cart.Items)
                .HasForeignKey(i => i.CartId);
        }

        private void ConfigureUser(EntityTypeBuilder<User> usersBuilder)
        {
            usersBuilder.ToTable("Users");
            usersBuilder.HasKey(u => u.Id);
            usersBuilder.Property(u => u.Email).HasMaxLength(40);
            usersBuilder.Property(u => u.Password).HasMaxLength(20);
            usersBuilder.HasCheckConstraint("Password", "LEN(Password) > 5");
          //  usersBuilder.HasCheckConstraint("DateOfBirth", "DateOfBirth > 1900-01-01 AND DateOfBirth < " +
            //    "CONVERT(date, GETDATE())");

            usersBuilder.HasOne(u => u.Cart)
                .WithOne(cart => cart.Owner)
                .HasForeignKey<User>(u => u.CartId);

            usersBuilder.HasMany(u => u.Orders)
                .WithMany(order => order.UsersInOrder)
                .UsingEntity(j => j.ToTable("UserOrder"));


        }

        private void ConfigureOrder(EntityTypeBuilder<Order> ordersBuilder)
        {
            ordersBuilder.ToTable("Orders");

            ordersBuilder.HasKey(o => o.Id);

            ordersBuilder.HasOne(o => o.Item)
                .WithOne(i => i.Order)
                .HasForeignKey<Order>(o => o.ItemId);
        }

        private void ConfigureCart(EntityTypeBuilder<Cart> cartBuilder)
        {
            cartBuilder.ToTable("Carts");
            cartBuilder.HasKey(cart => cart.Id);
        }        
    }
}

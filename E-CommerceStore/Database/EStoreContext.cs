using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using E_CommerceStore.Models.DatabaseModels;

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

        public DbSet<UserOrder> UserOrders { get; set; } = null!;

        public DbSet<BrandsTypes> BrandsTypes { get; set; } = null!;

        public DbSet<ItemCart> itemCarts { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

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
            builder.Entity<Review>(ConfigureReview);
        }
         
        private void ConfigureItemBrand(EntityTypeBuilder<ItemBrand> brandBuilder)
        {
            brandBuilder.ToTable("Brands");
            brandBuilder.HasKey(b => b.Id);
            brandBuilder.HasAlternateKey(b => b.Name);
            brandBuilder.Property(b => b.Name).HasMaxLength(50);
            brandBuilder.HasCheckConstraint("Name", "LEN(Name) > 2");

            brandBuilder.HasMany(b => b.ItemTypes)
                .WithMany(type => type.Brands)
                .UsingEntity<BrandsTypes>(bt=>bt.ToTable("BrandsTypes"));
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
            ipBuilder.HasCheckConstraint("Name", "LEN(Name) > 1");
            ipBuilder.Property(c => c.Name).HasMaxLength(40);

            ipBuilder.HasOne(c => c.ItemType)
                .WithMany(type => type.itemPropertyCategories)
                .HasForeignKey(c => c.ItemTypeId);
        }

        private void ConfigureItemProperty(EntityTypeBuilder<ItemProperty> propertyBuilder)
        {
            propertyBuilder.ToTable("Properties");
            propertyBuilder.HasKey(p => p.Id);
            propertyBuilder.Property(p => p.PropertyName).HasMaxLength(50);
            propertyBuilder.HasCheckConstraint("PropertyName", "LEN(PropertyName) > 0");
            propertyBuilder.Property(p => p.PropertyValue).HasMaxLength(50);
            propertyBuilder.HasCheckConstraint("PropertyValue", "LEN(PropertyValue) > 0");

            propertyBuilder.HasOne(p => p.PropertyCategory)
                .WithMany(c => c.CategoryProperties)
                .HasForeignKey(p => p.ItemPropertyCategoryId);

            propertyBuilder.HasOne(p => p.Item)
                .WithMany(i => i.PersonalProperties)
                .HasForeignKey(p => p.ItemId);
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
            itemsBuilder.Property(i => i.IsForSale).HasColumnType("BIT")
                .HasDefaultValue("TRUE").HasColumnName("IsSaling");

            
            itemsBuilder.HasOne(i => i.Brand)
                .WithMany(b => b.BrandItems)
                .HasForeignKey(i => i.BrandId);

            itemsBuilder.HasOne(i => i.ItemType)
                .WithMany(t => t.Items)
                .HasForeignKey(i => i.ItemTypeId);

            itemsBuilder.HasOne(i => i.ItemSeller)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SellerId);

            itemsBuilder.HasMany(i => i.Carts)
                .WithMany(cart => cart.Items)
                 .UsingEntity<ItemCart>(ic => ic.ToTable("ItemCart"));
        }

        private void ConfigureUser(EntityTypeBuilder<User> usersBuilder)
        {
            usersBuilder.ToTable("Users");
            usersBuilder.HasKey(u => u.Id);
          //  usersBuilder.HasAlternateKey(u => u.Email);
            usersBuilder.Property(u => u.Email).HasMaxLength(40);
            usersBuilder.Property(u => u.Password).HasMaxLength(20);
            usersBuilder.HasCheckConstraint("Password", "LEN(Password) > 5");
            usersBuilder.Property(u => u.RegisteredSince).HasDefaultValueSql("CONVERT(date, GETDATE())");
          //  usersBuilder.HasCheckConstraint("DateOfBirth", "DateOfBirth > 1900-01-01 AND DateOfBirth < " +
            //    "CONVERT(date, GETDATE())");

            usersBuilder.HasMany(u => u.Orders)
                .WithMany(order => order.UsersInOrder)
                .UsingEntity<UserOrder>(u=>u.ToTable("UsersOrders"));


        }

        private void ConfigureOrder(EntityTypeBuilder<Order> ordersBuilder)
        {
            ordersBuilder.ToTable("Orders");

            ordersBuilder.HasKey(o => o.Id);
            ordersBuilder.HasAlternateKey(o => o.UniqueToken);

            ordersBuilder.HasOne(o => o.Item)
                .WithMany(i => i.Orders)
                .HasForeignKey(o => o.ItemId);
        }

        private void ConfigureCart(EntityTypeBuilder<Cart> cartBuilder)
        {
            cartBuilder.ToTable("Carts");
            cartBuilder.HasKey(cart => cart.Id);

            cartBuilder.HasOne(cart => cart.Owner)
                .WithOne(user => user.Cart)
                .HasForeignKey<Cart>(cart => cart.OwnerId);
        }        

        private void ConfigureReview(EntityTypeBuilder<Review> reviewBuilder)
        {
            reviewBuilder.ToTable("Reviews");
            reviewBuilder.HasKey(r => r.Id);
            reviewBuilder.HasAlternateKey(r => new { r.ItemId, r.UserId });

            reviewBuilder.Property(r => r.shortComment).HasMaxLength(40);
            reviewBuilder.Property(r => r.longComment).HasMaxLength(150);
            reviewBuilder.Property(r => r.NumberOfLikes).HasDefaultValue(0);
            reviewBuilder.Property(r => r.NumberOfDislikes).HasDefaultValue(0);


            reviewBuilder.HasOne(r => r.reviewCreator)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);
            reviewBuilder.HasOne(r => r.ItemReviewed)
                .WithMany(i => i.Reviews)
                .HasForeignKey(r => r.ItemId);
        }
    }
}

﻿// <auto-generated />
using System;
using E_CommerceStore.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_CommerceStore.Migrations
{
    [DbContext(typeof(EStoreContext))]
    partial class EStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.BrandsTypes", b =>
                {
                    b.Property<int>("ItemBrandId")
                        .HasColumnType("int");

                    b.Property<int>("ItemTypeId")
                        .HasColumnType("int");

                    b.HasKey("ItemBrandId", "ItemTypeId");

                    b.HasIndex("ItemTypeId");

                    b.ToTable("BrandsTypes", (string)null);
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Carts", (string)null);
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("ImageSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("BrandId");

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("SellerId");

                    b.ToTable("Items", (string)null);

                    b.HasCheckConstraint("Name", "LEN(Name) > 5");

                    b.HasCheckConstraint("Price", "Price > 0");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Brands", (string)null);

                    b.HasCheckConstraint("Name", "LEN(Name) > 2", c => c.HasName("Name1"));
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemCart", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("CartId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemCart", (string)null);
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("ItemPropertyCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PropertyValue")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ItemPropertyCategoryId");

                    b.ToTable("Properties", (string)null);

                    b.HasCheckConstraint("PropertyName", "LEN(PropertyName) > 0");

                    b.HasCheckConstraint("PropertyValue", "LEN(PropertyValue) > 0");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemPropertyCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("ItemTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ItemTypeId");

                    b.ToTable("PropertyCategories", (string)null);

                    b.HasCheckConstraint("Name", "LEN(Name) > 3", c => c.HasName("Name2"));
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Types", (string)null);

                    b.HasCheckConstraint("Name", "LEN(Name) > 3", c => c.HasName("Name3"));
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("UniqueToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasAlternateKey("UniqueToken");

                    b.HasIndex("ItemId");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountImageSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("RegisteredSince")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CONVERT(date, GETDATE())");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int?>("country")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasCheckConstraint("Password", "LEN(Password) > 5");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.UserOrder", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersOrders", (string)null);
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.BrandsTypes", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.ItemBrand", "Brand")
                        .WithMany()
                        .HasForeignKey("ItemBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.ItemType", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Cart", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.User", "Owner")
                        .WithOne("Cart")
                        .HasForeignKey("E_CommerceStore.Models.DatabaseModels.Cart", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Item", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.ItemBrand", "Brand")
                        .WithMany("BrandItems")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.ItemType", "ItemType")
                        .WithMany("Items")
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.User", "ItemSeller")
                        .WithMany("Items")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("ItemSeller");

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemCart", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemProperty", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.Item", "Item")
                        .WithMany("PersonalProperties")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.ItemPropertyCategory", "PropertyCategory")
                        .WithMany("CategoryProperties")
                        .HasForeignKey("ItemPropertyCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("PropertyCategory");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemPropertyCategory", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.Item", null)
                        .WithMany("Categories")
                        .HasForeignKey("ItemId");

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.ItemType", "ItemType")
                        .WithMany("itemPropertyCategories")
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Order", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.Item", "Item")
                        .WithMany("Orders")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.UserOrder", b =>
                {
                    b.HasOne("E_CommerceStore.Models.DatabaseModels.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_CommerceStore.Models.DatabaseModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.Item", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Orders");

                    b.Navigation("PersonalProperties");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemBrand", b =>
                {
                    b.Navigation("BrandItems");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemPropertyCategory", b =>
                {
                    b.Navigation("CategoryProperties");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.ItemType", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("itemPropertyCategories");
                });

            modelBuilder.Entity("E_CommerceStore.Models.DatabaseModels.User", b =>
                {
                    b.Navigation("Cart");

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}

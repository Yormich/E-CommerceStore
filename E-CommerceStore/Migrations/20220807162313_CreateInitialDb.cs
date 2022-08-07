using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceStore.Migrations
{
    public partial class CreateInitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.UniqueConstraint("AK_Brands_Name", x => x.Name);
                    table.CheckConstraint("Name1", "LEN(Name) > 2");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                    table.UniqueConstraint("AK_Types_Name", x => x.Name);
                    table.CheckConstraint("Name3", "LEN(Name) > 3");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegisteredSince = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CONVERT(date, GETDATE())"),
                    country = table.Column<int>(type: "int", nullable: true),
                    AccountImageSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("Password", "LEN(Password) > 5");
                    table.ForeignKey(
                        name: "FK_Users_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandsTypes",
                columns: table => new
                {
                    ItemBrandId = table.Column<int>(type: "int", nullable: false),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandsTypes", x => new { x.ItemBrandId, x.ItemTypeId });
                    table.ForeignKey(
                        name: "FK_BrandsTypes_Brands_ItemBrandId",
                        column: x => x.ItemBrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandsTypes_Types_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Price = table.Column<decimal>(type: "decimal", nullable: false),
                    ImageSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.UniqueConstraint("AK_Items_Name", x => x.Name);
                    table.CheckConstraint("Name", "LEN(Name) > 5");
                    table.CheckConstraint("Price", "Price > 0");
                    table.ForeignKey(
                        name: "FK_Items_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Types_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCategories", x => x.Id);
                    table.CheckConstraint("Name2", "LEN(Name) > 3");
                    table.ForeignKey(
                        name: "FK_PropertyCategories_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropertyCategories_Types_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersOrders",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersOrders", x => new { x.OrderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UsersOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PropertyValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemPropertyCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.CheckConstraint("PropertyName", "LEN(PropertyName) > 0");
                    table.CheckConstraint("PropertyValue", "LEN(PropertyValue) > 0");
                    table.ForeignKey(
                        name: "FK_Properties_PropertyCategories_ItemPropertyCategoryId",
                        column: x => x.ItemPropertyCategoryId,
                        principalTable: "PropertyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandsTypes_ItemTypeId",
                table: "BrandsTypes",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CartId",
                table: "Items",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SellerId",
                table: "Items",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ItemId",
                table: "Orders",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ItemPropertyCategoryId",
                table: "Properties",
                column: "ItemPropertyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyCategories_ItemId",
                table: "PropertyCategories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyCategories_ItemTypeId",
                table: "PropertyCategories",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CartId",
                table: "Users",
                column: "CartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersOrders_UserId",
                table: "UsersOrders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandsTypes");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "UsersOrders");

            migrationBuilder.DropTable(
                name: "PropertyCategories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Carts");
        }
    }
}

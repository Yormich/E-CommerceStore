using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceStore.Migrations
{
    public partial class UpdateItemPropertyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ItemId",
                table: "Properties",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Items_ItemId",
                table: "Properties",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Items_ItemId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_ItemId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Properties");
        }
    }
}

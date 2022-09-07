using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceStore.Migrations
{
    public partial class RemoveSomeReviewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfDislikes",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "NumberOfLikes",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "shortComment",
                table: "Reviews",
                newName: "ShortComment");

            migrationBuilder.RenameColumn(
                name: "longComment",
                table: "Reviews",
                newName: "LongComment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortComment",
                table: "Reviews",
                newName: "shortComment");

            migrationBuilder.RenameColumn(
                name: "LongComment",
                table: "Reviews",
                newName: "longComment");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDislikes",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLikes",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

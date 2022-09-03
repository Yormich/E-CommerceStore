using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceStore.Migrations
{
    public partial class UpdateConstraintForPropertyCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Name2",
                table: "PropertyCategories");

            migrationBuilder.AddCheckConstraint(
                name: "Name2",
                table: "PropertyCategories",
                sql: "LEN(Name) > 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Name2",
                table: "PropertyCategories");

            migrationBuilder.AddCheckConstraint(
                name: "Name2",
                table: "PropertyCategories",
                sql: "LEN(Name) > 3");
        }
    }
}

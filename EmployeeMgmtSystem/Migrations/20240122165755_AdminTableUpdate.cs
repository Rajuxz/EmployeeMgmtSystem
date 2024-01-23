using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeMgmtSystem.Migrations
{
    public partial class AdminTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profile",
                table: "Admins",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile",
                table: "Admins");
        }
    }
}

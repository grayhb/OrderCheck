using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderCheck.DAL.Migrations
{
    public partial class AddCheckImagePathInDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckImagePath",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckImagePath",
                table: "Documents");
        }
    }
}

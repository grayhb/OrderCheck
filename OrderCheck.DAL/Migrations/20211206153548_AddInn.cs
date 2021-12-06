using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderCheck.DAL.Migrations
{
    public partial class AddInn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Inn",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inn",
                table: "Organizations");
        }
    }
}

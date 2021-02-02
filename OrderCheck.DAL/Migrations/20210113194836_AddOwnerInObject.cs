using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderCheck.DAL.Migrations
{
    public partial class AddOwnerInObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Objects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_OwnerId",
                table: "Objects",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_AspNetUsers_OwnerId",
                table: "Objects",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_AspNetUsers_OwnerId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_OwnerId",
                table: "Objects");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Objects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderCheck.DAL.Migrations
{
    public partial class RenameObjectToEstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.CreateTable(
                name: "Estates",
                columns: table => new
                {
                    EstateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstateAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estates", x => x.EstateId);
                    table.ForeignKey(
                        name: "FK_Estates_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estates_OwnerId",
                table: "Estates",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estates");

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    ObjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.ObjectId);
                    table.ForeignKey(
                        name: "FK_Objects_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Objects_OwnerId",
                table: "Objects",
                column: "OwnerId");
        }
    }
}

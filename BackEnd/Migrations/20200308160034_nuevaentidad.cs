using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class nuevaentidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCommerce",
                columns: table => new
                {
                    IdUser = table.Column<string>(nullable: false),
                    CommerceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommerce", x => new { x.CommerceID, x.IdUser });
                    table.UniqueConstraint("AK_UserCommerce_IdUser", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_UserCommerce_Commerce_CommerceID",
                        column: x => x.CommerceID,
                        principalTable: "Commerce",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCommerce_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCommerce");
        }
    }
}

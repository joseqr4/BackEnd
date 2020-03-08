using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class nuevaentidad2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserCommerce_IdUser",
                table: "UserCommerce");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommerce_IdUser",
                table: "UserCommerce",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCommerce_IdUser",
                table: "UserCommerce");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserCommerce_IdUser",
                table: "UserCommerce",
                column: "IdUser");
        }
    }
}

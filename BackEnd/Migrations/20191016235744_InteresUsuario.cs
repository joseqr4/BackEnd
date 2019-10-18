using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class InteresUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InteresesUsuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(nullable: false),
                    IdInteres = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteresesUsuarios", x => new { x.IdUsuario, x.IdInteres });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InteresesUsuarios");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InteresesUsuarios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InteresesUsuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(nullable: false),
                    InteresesFK = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteresesUsuarios", x => new { x.IdUsuario, x.InteresesFK });
                    table.ForeignKey(
                        name: "FK_InteresesUsuarios_Intereses_InteresesFK",
                        column: x => x.InteresesFK,
                        principalTable: "Intereses",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteresesUsuarios_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InteresesUsuarios_InteresesFK",
                table: "InteresesUsuarios",
                column: "InteresesFK");

            migrationBuilder.CreateIndex(
                name: "IX_InteresesUsuarios_UsuarioId",
                table: "InteresesUsuarios",
                column: "UsuarioId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class InteresUsuario2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdInteres",
                table: "InteresesUsuarios",
                newName: "InteresesFK");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "InteresesUsuarios",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteresesUsuarios_InteresesFK",
                table: "InteresesUsuarios",
                column: "InteresesFK");

            migrationBuilder.CreateIndex(
                name: "IX_InteresesUsuarios_UsuarioId",
                table: "InteresesUsuarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_InteresesUsuarios_Intereses_InteresesFK",
                table: "InteresesUsuarios",
                column: "InteresesFK",
                principalTable: "Intereses",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InteresesUsuarios_AspNetUsers_UsuarioId",
                table: "InteresesUsuarios",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InteresesUsuarios_Intereses_InteresesFK",
                table: "InteresesUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_InteresesUsuarios_AspNetUsers_UsuarioId",
                table: "InteresesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_InteresesUsuarios_InteresesFK",
                table: "InteresesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_InteresesUsuarios_UsuarioId",
                table: "InteresesUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "InteresesUsuarios");

            migrationBuilder.RenameColumn(
                name: "InteresesFK",
                table: "InteresesUsuarios",
                newName: "IdInteres");
        }
    }
}

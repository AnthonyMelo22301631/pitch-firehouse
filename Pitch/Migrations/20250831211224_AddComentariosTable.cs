using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pitch.Migrations
{
    /// <inheritdoc />
    public partial class AddComentariosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_AspNetUsers_UserId",
                table: "Comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Eventos_EventoId",
                table: "Comentario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentario",
                table: "Comentario");

            migrationBuilder.RenameTable(
                name: "Comentario",
                newName: "Comentarios");

            migrationBuilder.RenameIndex(
                name: "IX_Comentario_UserId",
                table: "Comentarios",
                newName: "IX_Comentarios_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comentario_EventoId",
                table: "Comentarios",
                newName: "IX_Comentarios_EventoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_AspNetUsers_UserId",
                table: "Comentarios",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Eventos_EventoId",
                table: "Comentarios",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_AspNetUsers_UserId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Eventos_EventoId",
                table: "Comentarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios");

            migrationBuilder.RenameTable(
                name: "Comentarios",
                newName: "Comentario");

            migrationBuilder.RenameIndex(
                name: "IX_Comentarios_UserId",
                table: "Comentario",
                newName: "IX_Comentario_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comentarios_EventoId",
                table: "Comentario",
                newName: "IX_Comentario_EventoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentario",
                table: "Comentario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_AspNetUsers_UserId",
                table: "Comentario",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Eventos_EventoId",
                table: "Comentario",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Core.Migrations
{
    /// <inheritdoc />
    public partial class AdequarEntityAutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_AspNetUsers_AutorId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AutorId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Biografia",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "AutorId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Biografia = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AutorId",
                table: "AspNetUsers",
                column: "AutorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Autores_AutorId",
                table: "AspNetUsers",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Autores_AutorId",
                table: "Comentarios",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Autores_AutorId",
                table: "Posts",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Autores_AutorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Autores_AutorId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Autores_AutorId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AutorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Biografia",
                table: "AspNetUsers",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AspNetUsers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_AspNetUsers_AutorId",
                table: "Comentarios",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AutorId",
                table: "Posts",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

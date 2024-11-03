using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Core.Migrations
{
    public partial class AdjustAspNetUserClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AspNetUserClaims",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity", "1, 1"); // Define auto-increment no SQL Server
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AspNetUserClaims",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true); // Reverte a anotação, se necessário
        }
    }

}

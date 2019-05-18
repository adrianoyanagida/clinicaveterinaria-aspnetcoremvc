using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoClinicaASPNETCore.Migrations
{
    public partial class CPFUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CPF",
                table: "AspNetUsers",
                column: "CPF",
                unique: true,
                filter: "[CPF] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CPF",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

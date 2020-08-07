using Microsoft.EntityFrameworkCore.Migrations;

namespace AlbertEinstein.Migrations
{
    public partial class mudancas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Exames_ExamesId",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_ExamesId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "ExamesId",
                table: "Consultas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamesId",
                table: "Consultas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_ExamesId",
                table: "Consultas",
                column: "ExamesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Exames_ExamesId",
                table: "Consultas",
                column: "ExamesId",
                principalTable: "Exames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

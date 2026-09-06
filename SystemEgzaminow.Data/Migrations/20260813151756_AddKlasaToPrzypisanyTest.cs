using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKlasaToPrzypisanyTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KlasaId",
                table: "PrzypisaneTesty",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrzypisaneTesty_KlasaId",
                table: "PrzypisaneTesty",
                column: "KlasaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrzypisaneTesty_Klasy_KlasaId",
                table: "PrzypisaneTesty",
                column: "KlasaId",
                principalTable: "Klasy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrzypisaneTesty_Klasy_KlasaId",
                table: "PrzypisaneTesty");

            migrationBuilder.DropIndex(
                name: "IX_PrzypisaneTesty_KlasaId",
                table: "PrzypisaneTesty");

            migrationBuilder.DropColumn(
                name: "KlasaId",
                table: "PrzypisaneTesty");
        }
    }
}
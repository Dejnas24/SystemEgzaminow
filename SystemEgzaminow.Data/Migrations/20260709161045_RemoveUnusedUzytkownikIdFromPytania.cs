using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedUzytkownikIdFromPytania : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pytania_Uzytkownicy_UzytkownikId",
                table: "Pytania");

            migrationBuilder.DropIndex(
                name: "IX_Pytania_UzytkownikId",
                table: "Pytania");

            migrationBuilder.DropColumn(
                name: "UzytkownikId",
                table: "Pytania");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UzytkownikId",
                table: "Pytania",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_UzytkownikId",
                table: "Pytania",
                column: "UzytkownikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pytania_Uzytkownicy_UzytkownikId",
                table: "Pytania",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id");
        }
    }
}
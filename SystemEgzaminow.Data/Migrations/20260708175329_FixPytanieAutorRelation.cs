using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPytanieAutorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pytania_Uzytkownicy_AutorId",
                table: "Pytania");

            migrationBuilder.DropIndex(
                name: "IX_Pytania_AutorId",
                table: "Pytania");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Pytania");

            migrationBuilder.AddColumn<int>(
                name: "UzytkownikId",
                table: "Pytania",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_IdAutora",
                table: "Pytania",
                column: "IdAutora");

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_UzytkownikId",
                table: "Pytania",
                column: "UzytkownikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pytania_Uzytkownicy_IdAutora",
                table: "Pytania",
                column: "IdAutora",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pytania_Uzytkownicy_UzytkownikId",
                table: "Pytania",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pytania_Uzytkownicy_IdAutora",
                table: "Pytania");

            migrationBuilder.DropForeignKey(
                name: "FK_Pytania_Uzytkownicy_UzytkownikId",
                table: "Pytania");

            migrationBuilder.DropIndex(
                name: "IX_Pytania_IdAutora",
                table: "Pytania");

            migrationBuilder.DropIndex(
                name: "IX_Pytania_UzytkownikId",
                table: "Pytania");

            migrationBuilder.DropColumn(
                name: "UzytkownikId",
                table: "Pytania");

            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Pytania",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_AutorId",
                table: "Pytania",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pytania_Uzytkownicy_AutorId",
                table: "Pytania",
                column: "AutorId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
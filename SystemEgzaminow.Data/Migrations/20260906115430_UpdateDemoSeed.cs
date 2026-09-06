using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDemoSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Klasy",
                columns: new[] { "Id", "NazwaKlasy", "RokSzkolny" },
                values: new object[] { 19, "1A", "2026/2027" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nazwa",
                value: "Uczeń");

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 1,
                column: "Haslo",
                value: "$2a$11$OBMFBVR69BHicXCSfvM76.DOA56UDvfCQvQT9YpCxiqtFTRX420Xi");

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 2,
                column: "Haslo",
                value: "$2a$11$GHgwaN9fnSfe5a/N0NN9WO0UfTv4QRM4YEydXYaw/CDVG4I.9paTG");

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Haslo", "KlasaId" },
                values: new object[] { "$2a$11$vLwHLtnJMgFIcMRa8apt0OTG7OPFNloOSpf2rruTakDTcr0ghA71C", 19 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Klasy",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nazwa",
                value: "Uczen");

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 1,
                column: "Haslo",
                value: "admin123");

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 2,
                column: "Haslo",
                value: "123");

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Haslo", "KlasaId" },
                values: new object[] { "123", null });
        }
    }
}

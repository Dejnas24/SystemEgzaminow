using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTestResultsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WynikiTestow_Testy_TestId",
                table: "WynikiTestow");

            migrationBuilder.DropIndex(
                name: "IX_TestPytania_TestId",
                table: "TestPytania");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "WynikiTestow",
                newName: "PrzypisanyTestId");

            migrationBuilder.RenameIndex(
                name: "IX_WynikiTestow_TestId",
                table: "WynikiTestow",
                newName: "IX_WynikiTestow_PrzypisanyTestId");

            migrationBuilder.AddColumn<int>(
                name: "MaksymalnaLiczbaPunktow",
                table: "WynikiTestow",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTypuTestu",
                table: "Testy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PrzypisaneTesty",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "CzyPokazacWynikPoZakonczeniu",
                table: "PrzypisaneTesty",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RozwiazanePytania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPytaniaZrodlowego = table.Column<int>(type: "int", nullable: true),
                    TrescPytania = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    TypPytania = table.Column<int>(type: "int", nullable: false),
                    LiczbaPunktowMax = table.Column<int>(type: "int", nullable: false),
                    LiczbaPunktowZdobytych = table.Column<int>(type: "int", nullable: false),
                    IdAutoraPytania = table.Column<int>(type: "int", nullable: false),
                    Kolejnosc = table.Column<int>(type: "int", nullable: false),
                    IdWynikuTestu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RozwiazanePytania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RozwiazanePytania_WynikiTestow_IdWynikuTestu",
                        column: x => x.IdWynikuTestu,
                        principalTable: "WynikiTestow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypyTestow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazwaTypu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypyTestow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RozwiazaneOdpowiedzi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRozwiazanegoPytania = table.Column<int>(type: "int", nullable: false),
                    IdOdpowiedziZrodlowej = table.Column<int>(type: "int", nullable: true),
                    TrescOdpowiedzi = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CzyPoprawna = table.Column<bool>(type: "bit", nullable: false),
                    CzyWybranaPrzezUcznia = table.Column<bool>(type: "bit", nullable: false),
                    TrescOdpowiedziUcznia = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Kolejnosc = table.Column<int>(type: "int", nullable: false),
                    LiczbaPunktow = table.Column<int>(type: "int", nullable: false),
                    KomentarzNauczyciela = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RozwiazaneOdpowiedzi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RozwiazaneOdpowiedzi_RozwiazanePytania_IdRozwiazanegoPytania",
                        column: x => x.IdRozwiazanegoPytania,
                        principalTable: "RozwiazanePytania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TypyTestow",
                columns: new[] { "Id", "NazwaTypu" },
                values: new object[,]
                {
                    { 1, "Kartkówka" },
                    { 2, "Sprawdzian" },
                    { 3, "Test ćwiczeniowy" },
                    { 4, "Olimpiada" },
                    { 5, "Wejściówka" },
                    { 6, "Kolokwium" },
                    { 7, "Egzamin" },
                    { 8, "Test certyfikacyjny" },
                    { 9, "Egzamin certyfikacyjny" },
                    { 10, "Inny" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Testy_IdTypuTestu",
                table: "Testy",
                column: "IdTypuTestu");

            migrationBuilder.CreateIndex(
                name: "IX_TestPytania_TestId_PytanieId",
                table: "TestPytania",
                columns: new[] { "TestId", "PytanieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RozwiazaneOdpowiedzi_IdRozwiazanegoPytania",
                table: "RozwiazaneOdpowiedzi",
                column: "IdRozwiazanegoPytania");

            migrationBuilder.CreateIndex(
                name: "IX_RozwiazanePytania_IdWynikuTestu_Kolejnosc",
                table: "RozwiazanePytania",
                columns: new[] { "IdWynikuTestu", "Kolejnosc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypyTestow_NazwaTypu",
                table: "TypyTestow",
                column: "NazwaTypu",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Testy_TypyTestow_IdTypuTestu",
                table: "Testy",
                column: "IdTypuTestu",
                principalTable: "TypyTestow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WynikiTestow_PrzypisaneTesty_PrzypisanyTestId",
                table: "WynikiTestow",
                column: "PrzypisanyTestId",
                principalTable: "PrzypisaneTesty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testy_TypyTestow_IdTypuTestu",
                table: "Testy");

            migrationBuilder.DropForeignKey(
                name: "FK_WynikiTestow_PrzypisaneTesty_PrzypisanyTestId",
                table: "WynikiTestow");

            migrationBuilder.DropTable(
                name: "RozwiazaneOdpowiedzi");

            migrationBuilder.DropTable(
                name: "TypyTestow");

            migrationBuilder.DropTable(
                name: "RozwiazanePytania");

            migrationBuilder.DropIndex(
                name: "IX_Testy_IdTypuTestu",
                table: "Testy");

            migrationBuilder.DropIndex(
                name: "IX_TestPytania_TestId_PytanieId",
                table: "TestPytania");

            migrationBuilder.DropColumn(
                name: "MaksymalnaLiczbaPunktow",
                table: "WynikiTestow");

            migrationBuilder.DropColumn(
                name: "IdTypuTestu",
                table: "Testy");

            migrationBuilder.DropColumn(
                name: "CzyPokazacWynikPoZakonczeniu",
                table: "PrzypisaneTesty");

            migrationBuilder.RenameColumn(
                name: "PrzypisanyTestId",
                table: "WynikiTestow",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_WynikiTestow_PrzypisanyTestId",
                table: "WynikiTestow",
                newName: "IX_WynikiTestow_TestId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PrzypisaneTesty",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TestPytania_TestId",
                table: "TestPytania",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_WynikiTestow_Testy_TestId",
                table: "WynikiTestow",
                column: "TestId",
                principalTable: "Testy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFullExamSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogiLogowan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    DataLogowania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataWylogowania = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CzySukces = table.Column<bool>(type: "bit", nullable: false),
                    Sesja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogiLogowan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogiLogowan_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pytania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrescPytania = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypPytania = table.Column<int>(type: "int", nullable: false),
                    LiczbaPunktow = table.Column<int>(type: "int", nullable: false),
                    IdAutora = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pytania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pytania_Uzytkownicy_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Testy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgZadania = table.Column<int>(type: "int", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CzasTrwaniaMinuty = table.Column<int>(type: "int", nullable: false),
                    IdAutora = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testy_Uzytkownicy_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odpowiedzi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrescOdpowiedzi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CzyPoprawna = table.Column<bool>(type: "bit", nullable: false),
                    LiczbaPunktow = table.Column<int>(type: "int", nullable: true),
                    PytanieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odpowiedzi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odpowiedzi_Pytania_PytanieId",
                        column: x => x.PytanieId,
                        principalTable: "Pytania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrzypisaneTesty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    UczenId = table.Column<int>(type: "int", nullable: false),
                    NauczycielId = table.Column<int>(type: "int", nullable: false),
                    DataPrzypisania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDostepnosci = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataWygasniecia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrzypisaneTesty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrzypisaneTesty_Testy_TestId",
                        column: x => x.TestId,
                        principalTable: "Testy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrzypisaneTesty_Uzytkownicy_NauczycielId",
                        column: x => x.NauczycielId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrzypisaneTesty_Uzytkownicy_UczenId",
                        column: x => x.UczenId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestPytania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    PytanieId = table.Column<int>(type: "int", nullable: false),
                    NumerKolejnosci = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPytania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestPytania_Pytania_PytanieId",
                        column: x => x.PytanieId,
                        principalTable: "Pytania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestPytania_Testy_TestId",
                        column: x => x.TestId,
                        principalTable: "Testy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WynikiTestow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    LiczbaPunktow = table.Column<int>(type: "int", nullable: false),
                    Procent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Ocena = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    CzyZdane = table.Column<bool>(type: "bit", nullable: false),
                    CzyPrubny = table.Column<bool>(type: "bit", nullable: true),
                    DataRozpoczecia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataZakonczenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CzasRozwiazywaniaMinuty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WynikiTestow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WynikiTestow_Testy_TestId",
                        column: x => x.TestId,
                        principalTable: "Testy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WynikiTestow_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogiRozwiazywaniaTestu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrzypisanyTestId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    DataZdarzenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypZdarzenia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogiRozwiazywaniaTestu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogiRozwiazywaniaTestu_PrzypisaneTesty_PrzypisanyTestId",
                        column: x => x.PrzypisanyTestId,
                        principalTable: "PrzypisaneTesty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogiRozwiazywaniaTestu_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Uzytkownicy",
                columns: new[] { "Id", "Haslo", "Imie", "Login", "Nazwisko", "RolaId" },
                values: new object[,]
                {
                    { 1, "admin123", "Admin", "admin", "Systemowy", 1 },
                    { 2, "123", "Jan", "nauczyciel", "Kowalski", 2 },
                    { 3, "123", "Adam", "uczen", "Nowak", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogiLogowan_UzytkownikId",
                table: "LogiLogowan",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_LogiRozwiazywaniaTestu_PrzypisanyTestId",
                table: "LogiRozwiazywaniaTestu",
                column: "PrzypisanyTestId");

            migrationBuilder.CreateIndex(
                name: "IX_LogiRozwiazywaniaTestu_UzytkownikId",
                table: "LogiRozwiazywaniaTestu",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Odpowiedzi_PytanieId",
                table: "Odpowiedzi",
                column: "PytanieId");

            migrationBuilder.CreateIndex(
                name: "IX_PrzypisaneTesty_NauczycielId",
                table: "PrzypisaneTesty",
                column: "NauczycielId");

            migrationBuilder.CreateIndex(
                name: "IX_PrzypisaneTesty_TestId",
                table: "PrzypisaneTesty",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_PrzypisaneTesty_UczenId",
                table: "PrzypisaneTesty",
                column: "UczenId");

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_AutorId",
                table: "Pytania",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPytania_PytanieId",
                table: "TestPytania",
                column: "PytanieId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPytania_TestId",
                table: "TestPytania",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Testy_AutorId",
                table: "Testy",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_WynikiTestow_TestId",
                table: "WynikiTestow",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_WynikiTestow_UzytkownikId",
                table: "WynikiTestow",
                column: "UzytkownikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogiLogowan");

            migrationBuilder.DropTable(
                name: "LogiRozwiazywaniaTestu");

            migrationBuilder.DropTable(
                name: "Odpowiedzi");

            migrationBuilder.DropTable(
                name: "TestPytania");

            migrationBuilder.DropTable(
                name: "WynikiTestow");

            migrationBuilder.DropTable(
                name: "PrzypisaneTesty");

            migrationBuilder.DropTable(
                name: "Pytania");

            migrationBuilder.DropTable(
                name: "Testy");

            migrationBuilder.DeleteData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
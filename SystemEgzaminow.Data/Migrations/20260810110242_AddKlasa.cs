using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKlasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KlasaId",
                table: "Uzytkownicy",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Klasy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazwaKlasy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RokSzkolny = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klasy", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 1,
                column: "KlasaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 2,
                column: "KlasaId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 3,
                column: "KlasaId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_KlasaId",
                table: "Uzytkownicy",
                column: "KlasaId");

            migrationBuilder.CreateIndex(
                name: "IX_Klasy_NazwaKlasy_RokSzkolny",
                table: "Klasy",
                columns: new[] { "NazwaKlasy", "RokSzkolny" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Uzytkownicy_Klasy_KlasaId",
                table: "Uzytkownicy",
                column: "KlasaId",
                principalTable: "Klasy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uzytkownicy_Klasy_KlasaId",
                table: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "Klasy");

            migrationBuilder.DropIndex(
                name: "IX_Uzytkownicy_KlasaId",
                table: "Uzytkownicy");

            migrationBuilder.DropColumn(
                name: "KlasaId",
                table: "Uzytkownicy");
        }
    }
}
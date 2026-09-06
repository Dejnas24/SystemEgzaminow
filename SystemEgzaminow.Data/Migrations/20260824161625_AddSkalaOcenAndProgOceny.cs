using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSkalaOcenAndProgOceny : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkaleOcen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTypuTestu = table.Column<int>(type: "int", nullable: false),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: true),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CzyAktywna = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkaleOcen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkaleOcen_TypyTestow_IdTypuTestu",
                        column: x => x.IdTypuTestu,
                        principalTable: "TypyTestow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkaleOcen_Uzytkownicy_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgiOcen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSkaliOcen = table.Column<int>(type: "int", nullable: false),
                    Ocena = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: false),
                    ProgOd = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ProgDo = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgiOcen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgiOcen_SkaleOcen_IdSkaliOcen",
                        column: x => x.IdSkaliOcen,
                        principalTable: "SkaleOcen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgiOcen_IdSkaliOcen_ProgOd_ProgDo",
                table: "ProgiOcen",
                columns: new[] { "IdSkaliOcen", "ProgOd", "ProgDo" });

            migrationBuilder.CreateIndex(
                name: "IX_SkaleOcen_IdTypuTestu_IdUzytkownika_CzyAktywna",
                table: "SkaleOcen",
                columns: new[] { "IdTypuTestu", "IdUzytkownika", "CzyAktywna" });

            migrationBuilder.CreateIndex(
                name: "IX_SkaleOcen_IdUzytkownika",
                table: "SkaleOcen",
                column: "IdUzytkownika");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgiOcen");

            migrationBuilder.DropTable(
                name: "SkaleOcen");
        }
    }
}
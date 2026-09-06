using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProgZadaniaToProgZdania : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
        name: "ProgZadania",
        table: "Testy",
        newName: "ProgZdania");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
       name: "ProgZdania",
       table: "Testy",
       newName: "ProgZadania");
        }
    }
}
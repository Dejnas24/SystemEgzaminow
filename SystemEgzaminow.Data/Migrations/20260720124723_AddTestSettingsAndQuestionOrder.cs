using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTestSettingsAndQuestionOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LosujKolejnoscOdpowiedzi",
                table: "Testy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LosujKolejnoscPytan",
                table: "Testy",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LosujKolejnoscOdpowiedzi",
                table: "Testy");

            migrationBuilder.DropColumn(
                name: "LosujKolejnoscPytan",
                table: "Testy");
        }
    }
}
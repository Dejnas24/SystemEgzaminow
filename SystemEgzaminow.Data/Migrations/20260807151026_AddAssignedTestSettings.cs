using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedTestSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InformacjaKoncowa",
                table: "PrzypisaneTesty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InformacjaStartowa",
                table: "PrzypisaneTesty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiczbaProb",
                table: "PrzypisaneTesty",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InformacjaKoncowa",
                table: "PrzypisaneTesty");

            migrationBuilder.DropColumn(
                name: "InformacjaStartowa",
                table: "PrzypisaneTesty");

            migrationBuilder.DropColumn(
                name: "LiczbaProb",
                table: "PrzypisaneTesty");
        }
    }
}
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSposobOcenianiaToPrzypisanyTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SposobOceniania",
                table: "PrzypisaneTesty",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SposobOceniania",
                table: "PrzypisaneTesty");
        }
    }
}
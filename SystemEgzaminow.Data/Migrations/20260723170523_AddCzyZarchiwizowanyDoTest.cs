using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCzyZarchiwizowanyDoTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CzyZarchiwizowany",
                table: "Testy",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CzyZarchiwizowany",
                table: "Testy");
        }
    }
}
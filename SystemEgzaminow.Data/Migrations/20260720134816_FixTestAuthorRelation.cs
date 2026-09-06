using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTestAuthorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testy_Uzytkownicy_AutorId",
                table: "Testy");

            migrationBuilder.DropIndex(
                name: "IX_Testy_AutorId",
                table: "Testy");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Testy");

            migrationBuilder.CreateIndex(
                name: "IX_Testy_IdAutora",
                table: "Testy",
                column: "IdAutora");

            migrationBuilder.AddForeignKey(
                name: "FK_Testy_Uzytkownicy_IdAutora",
                table: "Testy",
                column: "IdAutora",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testy_Uzytkownicy_IdAutora",
                table: "Testy");

            migrationBuilder.DropIndex(
                name: "IX_Testy_IdAutora",
                table: "Testy");

            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Testy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Testy_AutorId",
                table: "Testy",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Testy_Uzytkownicy_AutorId",
                table: "Testy",
                column: "AutorId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
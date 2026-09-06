using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemEgzaminow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogLogowaniaSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogiLogowan_Uzytkownicy_UzytkownikId",
                table: "LogiLogowan");

            migrationBuilder.AlterColumn<int>(
                name: "UzytkownikId",
                table: "LogiLogowan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Sesja",
                table: "LogiLogowan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LogiLogowan_Uzytkownicy_UzytkownikId",
                table: "LogiLogowan",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogiLogowan_Uzytkownicy_UzytkownikId",
                table: "LogiLogowan");

            migrationBuilder.AlterColumn<int>(
                name: "UzytkownikId",
                table: "LogiLogowan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sesja",
                table: "LogiLogowan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LogiLogowan_Uzytkownicy_UzytkownikId",
                table: "LogiLogowan",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
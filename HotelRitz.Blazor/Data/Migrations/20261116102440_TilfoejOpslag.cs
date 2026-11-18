
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRitz.Blazor.Migrations
{
    public partial class TilfoejOpslag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Opretter tabellen til registrering af læste opslag
            migrationBuilder.CreateTable(
                name: "Opslagslaesninger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),

                    OpslagId = table.Column<int>(type: "INTEGER", nullable: false),

                    BrugerId = table.Column<string>(type: "TEXT", nullable: false),

                    LaesningsDato = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opslagslaesninger", x => x.Id);
                });

            // Tilføjer nye kolonner til den eksisterende opslagstavle
            migrationBuilder.AddColumn<bool>(
                name: "ErFaerdiggjort",
                table: "OpslagsTavler",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FaerdiggjortDato",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: true);

            // Gør de eksisterende tekstkolonner nullable
            migrationBuilder.AlterColumn<string>(
                name: "Titel",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Besked",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Forfatter",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opslagslaesninger");

            migrationBuilder.DropColumn(
                name: "ErFaerdiggjort",
                table: "OpslagsTavler");

            migrationBuilder.DropColumn(
                name: "FaerdiggjortDato",
                table: "OpslagsTavler");

            migrationBuilder.AlterColumn<string>(
                name: "Titel",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Besked",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Forfatter",
                table: "OpslagsTavler",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}

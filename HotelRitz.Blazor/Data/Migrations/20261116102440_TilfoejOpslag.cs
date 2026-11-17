using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRitz.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class TilfoejOpslag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Opslagslaesninger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OpslagId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrugerId = table.Column<string>(type: "TEXT", nullable: true),
                    LaesningsDato = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opslagslaesninger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpslagsTavler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titel = table.Column<string>(type: "TEXT", nullable: true),
                    Besked = table.Column<string>(type: "TEXT", nullable: true),
                    Forfatter = table.Column<string>(type: "TEXT", nullable: true),
                    Oprettelsesdato = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ErFaerdiggjort = table.Column<bool>(type: "INTEGER", nullable: false),
                    FaerdiggjortDato = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpslagsTavler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opslagslaesninger");

            migrationBuilder.DropTable(
                name: "OpslagsTavler");
        }
    }
}

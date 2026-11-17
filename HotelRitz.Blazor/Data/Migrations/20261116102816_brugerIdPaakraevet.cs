using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRitz.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class brugerIdPaakraevet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BrugerId",
                table: "Opslagslaesninger",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BrugerId",
                table: "Opslagslaesninger",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajesAPI.Migrations
{
    /// <inheritdoc />
    public partial class SetLatLongPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "travels",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "travels",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "travels");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "travels");
        }
    }
}

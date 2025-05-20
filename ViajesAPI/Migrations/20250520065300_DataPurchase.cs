using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajesAPI.Migrations
{
    /// <inheritdoc />
    public partial class DataPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destino",
                table: "purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InitDate",
                table: "purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "purchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destino",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "InitDate",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "purchases");
        }
    }
}

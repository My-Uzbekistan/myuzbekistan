using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class ESimPackageAdditionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<long>>(
                name: "Locals",
                table: "ESimPackages",
                type: "bigint[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Text",
                table: "ESimPackages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Voice",
                table: "ESimPackages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locals",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "Voice",
                table: "ESimPackages");
        }
    }
}

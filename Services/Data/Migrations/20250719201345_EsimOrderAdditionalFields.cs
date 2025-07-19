using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class EsimOrderAdditionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coverage",
                table: "ESimPackages",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ESimPackages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                 name: "Info",
                 table: "ESimPackages",
                 type: "text[]",
                 nullable: true,
                 defaultValue: new List<string>());

            migrationBuilder.AddColumn<bool>(
                name: "IsRoaming",
                table: "ESimPackages",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatorName",
                table: "ESimPackages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherInfo",
                table: "ESimPackages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationDate",
                table: "ESimOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "ESimOrders",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coverage",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "IsRoaming",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "OperatorName",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "OtherInfo",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "ESimOrders");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "ESimOrders");
        }
    }
}

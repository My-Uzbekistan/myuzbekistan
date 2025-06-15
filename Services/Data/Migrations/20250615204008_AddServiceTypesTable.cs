using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceType",
                table: "MerchantCategories",
                newName: "ServiceTypeLocale");

            migrationBuilder.AddColumn<long>(
                name: "ServiceTypeId",
                table: "MerchantCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => new { x.Id, x.Locale });
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantCategories_ServiceTypeId_ServiceTypeLocale",
                table: "MerchantCategories",
                columns: new[] { "ServiceTypeId", "ServiceTypeLocale" });

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantCategories_ServiceTypes_ServiceTypeId_ServiceTypeLo~",
                table: "MerchantCategories",
                columns: new[] { "ServiceTypeId", "ServiceTypeLocale" },
                principalTable: "ServiceTypes",
                principalColumns: new[] { "Id", "Locale" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchantCategories_ServiceTypes_ServiceTypeId_ServiceTypeLo~",
                table: "MerchantCategories");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_MerchantCategories_ServiceTypeId_ServiceTypeLocale",
                table: "MerchantCategories");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "MerchantCategories");

            migrationBuilder.RenameColumn(
                name: "ServiceTypeLocale",
                table: "MerchantCategories",
                newName: "ServiceType");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocaleAndLocationColumnToMerchantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId",
                table: "Merchants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_Merchants_MerchantCategoryId",
                table: "Merchants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MerchantCategories",
                table: "MerchantCategories");

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Merchants",
                type: "geometry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MerchantCategoryLocale",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "MerchantCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants",
                columns: new[] { "Id", "Locale" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MerchantCategories",
                table: "MerchantCategories",
                columns: new[] { "Id", "Locale" });

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_MerchantCategoryId_MerchantCategoryLocale",
                table: "Merchants",
                columns: new[] { "MerchantCategoryId", "MerchantCategoryLocale" });

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId_MerchantCat~",
                table: "Merchants",
                columns: new[] { "MerchantCategoryId", "MerchantCategoryLocale" },
                principalTable: "MerchantCategories",
                principalColumns: new[] { "Id", "Locale" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId_MerchantCat~",
                table: "Merchants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_Merchants_MerchantCategoryId_MerchantCategoryLocale",
                table: "Merchants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MerchantCategories",
                table: "MerchantCategories");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "MerchantCategoryLocale",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "MerchantCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MerchantCategories",
                table: "MerchantCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_MerchantCategoryId",
                table: "Merchants",
                column: "MerchantCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId",
                table: "Merchants",
                column: "MerchantCategoryId",
                principalTable: "MerchantCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

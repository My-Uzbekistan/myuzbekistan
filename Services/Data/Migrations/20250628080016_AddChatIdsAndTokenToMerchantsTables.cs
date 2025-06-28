using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatIdsAndTokenToMerchantsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "ChatIds",
                table: "Merchants",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Merchants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "ChatIds",
                table: "MerchantCategories",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "MerchantCategories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatIds",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "ChatIds",
                table: "MerchantCategories");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "MerchantCategories");
        }
    }
}

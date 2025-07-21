using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFileEntitryToCardPrefixesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardPrefixes_Files_CardBrandId",
                table: "CardPrefixes");

            migrationBuilder.DropIndex(
                name: "IX_CardPrefixes_CardBrandId",
                table: "CardPrefixes");

            migrationBuilder.DropColumn(
                name: "CardBrandId",
                table: "CardPrefixes");

            migrationBuilder.AddColumn<string>(
                name: "CardBrand",
                table: "CardPrefixes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardBrand",
                table: "CardPrefixes");

            migrationBuilder.AddColumn<long>(
                name: "CardBrandId",
                table: "CardPrefixes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardPrefixes_CardBrandId",
                table: "CardPrefixes",
                column: "CardBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardPrefixes_Files_CardBrandId",
                table: "CardPrefixes",
                column: "CardBrandId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}

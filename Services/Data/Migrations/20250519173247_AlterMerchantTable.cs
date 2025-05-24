using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterMerchantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Merchants");

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Merchants",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_ImageId",
                table: "Merchants",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Files_ImageId",
                table: "Merchants",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Files_ImageId",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_Merchants_ImageId",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Merchants");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Merchants",
                type: "text",
                nullable: true);
        }
    }
}

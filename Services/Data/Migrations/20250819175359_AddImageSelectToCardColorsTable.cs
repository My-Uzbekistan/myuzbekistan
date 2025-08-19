using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageSelectToCardColorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "CardColors",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardColors_ImageId",
                table: "CardColors",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardColors_Files_ImageId",
                table: "CardColors",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardColors_Files_ImageId",
                table: "CardColors");

            migrationBuilder.DropIndex(
                name: "IX_CardColors_ImageId",
                table: "CardColors");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "CardColors");
        }
    }
}

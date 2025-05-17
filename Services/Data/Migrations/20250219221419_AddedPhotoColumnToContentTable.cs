using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhotoColumnToContentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PhotoId",
                table: "Contents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Files_PhotoId",
                table: "Contents",
                column: "PhotoId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Files_PhotoId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Contents");
        }
    }
}

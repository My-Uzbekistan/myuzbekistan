using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterContentPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents",
                column: "PhotoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents",
                column: "PhotoId",
                unique: true);
        }
    }
}

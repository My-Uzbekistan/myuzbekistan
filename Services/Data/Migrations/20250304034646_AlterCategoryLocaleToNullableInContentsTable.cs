using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterCategoryLocaleToNullableInContentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Categories_CategoryId_CategoryLocale",
                table: "Contents");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryLocale",
                table: "Contents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Categories_CategoryId_CategoryLocale",
                table: "Contents",
                columns: new[] { "CategoryId", "CategoryLocale" },
                principalTable: "Categories",
                principalColumns: new[] { "Id", "Locale" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Categories_CategoryId_CategoryLocale",
                table: "Contents");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryLocale",
                table: "Contents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Categories_CategoryId_CategoryLocale",
                table: "Contents",
                columns: new[] { "CategoryId", "CategoryLocale" },
                principalTable: "Categories",
                principalColumns: new[] { "Id", "Locale" });
        }
    }
}

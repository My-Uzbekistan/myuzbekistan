using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterRatingAverage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Contents_ContentEntityId_ContentEntityLocale",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ContentEntityLocale",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ContentEntityId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RatingAverage",
                table: "Contents",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Contents_ContentEntityId_ContentEntityLocale",
                table: "Reviews",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" },
                principalTable: "Contents",
                principalColumns: new[] { "Id", "Locale" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Contents_ContentEntityId_ContentEntityLocale",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ContentEntityLocale",
                table: "Reviews",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "ContentEntityId",
                table: "Reviews",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "RatingAverage",
                table: "Contents",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Contents_ContentEntityId_ContentEntityLocale",
                table: "Reviews",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" },
                principalTable: "Contents",
                principalColumns: new[] { "Id", "Locale" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignManyToManyToLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Contents_ContentEntityId_ContentEntityLocale",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_ContentEntityId_ContentEntityLocale",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "ContentEntityId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "ContentEntityLocale",
                table: "Languages");

            migrationBuilder.CreateTable(
                name: "ContentEntityLanguageEntity",
                columns: table => new
                {
                    ContentsId = table.Column<long>(type: "bigint", nullable: false),
                    ContentsLocale = table.Column<string>(type: "text", nullable: false),
                    LanguagesId = table.Column<long>(type: "bigint", nullable: false),
                    LanguagesLocale = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEntityLanguageEntity", x => new { x.ContentsId, x.ContentsLocale, x.LanguagesId, x.LanguagesLocale });
                    table.ForeignKey(
                        name: "FK_ContentEntityLanguageEntity_Contents_ContentsId_ContentsLoc~",
                        columns: x => new { x.ContentsId, x.ContentsLocale },
                        principalTable: "Contents",
                        principalColumns: new[] { "Id", "Locale" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentEntityLanguageEntity_Languages_LanguagesId_Languages~",
                        columns: x => new { x.LanguagesId, x.LanguagesLocale },
                        principalTable: "Languages",
                        principalColumns: new[] { "Id", "Locale" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentEntityLanguageEntity_LanguagesId_LanguagesLocale",
                table: "ContentEntityLanguageEntity",
                columns: new[] { "LanguagesId", "LanguagesLocale" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentEntityLanguageEntity");

            migrationBuilder.AddColumn<long>(
                name: "ContentEntityId",
                table: "Languages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentEntityLocale",
                table: "Languages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ContentEntityId_ContentEntityLocale",
                table: "Languages",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" });

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Contents_ContentEntityId_ContentEntityLocale",
                table: "Languages",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" },
                principalTable: "Contents",
                principalColumns: new[] { "Id", "Locale" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class FilesManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Contents_ContentEntityId1_ContentEntityLocale1",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Contents_ContentEntityId_ContentEntityLocale",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ContentEntityId_ContentEntityLocale",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ContentEntityId1_ContentEntityLocale1",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ContentEntityId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ContentEntityId1",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ContentEntityLocale",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ContentEntityLocale1",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "ContentEntityFileEntity",
                columns: table => new
                {
                    FilesId = table.Column<long>(type: "bigint", nullable: false),
                    ContentFilesId = table.Column<long>(type: "bigint", nullable: false),
                    ContentFilesLocale = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEntityFileEntity", x => new { x.FilesId, x.ContentFilesId, x.ContentFilesLocale });
                    table.ForeignKey(
                        name: "FK_ContentEntityFileEntity_Contents_ContentFilesId_ContentFile~",
                        columns: x => new { x.ContentFilesId, x.ContentFilesLocale },
                        principalTable: "Contents",
                        principalColumns: new[] { "Id", "Locale" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentEntityFileEntity_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentEntityFileEntity1",
                columns: table => new
                {
                    PhotosId = table.Column<long>(type: "bigint", nullable: false),
                    ContentPhotosId = table.Column<long>(type: "bigint", nullable: false),
                    ContentPhotosLocale = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEntityFileEntity1", x => new { x.PhotosId, x.ContentPhotosId, x.ContentPhotosLocale });
                    table.ForeignKey(
                        name: "FK_ContentEntityFileEntity1_Contents_ContentPhotosId_ContentPh~",
                        columns: x => new { x.ContentPhotosId, x.ContentPhotosLocale },
                        principalTable: "Contents",
                        principalColumns: new[] { "Id", "Locale" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentEntityFileEntity1_Files_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentEntityFileEntity_ContentFilesId_ContentFilesLocale",
                table: "ContentEntityFileEntity",
                columns: new[] { "ContentFilesId", "ContentFilesLocale" });

            migrationBuilder.CreateIndex(
                name: "IX_ContentEntityFileEntity1_ContentPhotosId_ContentPhotosLocale",
                table: "ContentEntityFileEntity1",
                columns: new[] { "ContentPhotosId", "ContentPhotosLocale" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentEntityFileEntity");

            migrationBuilder.DropTable(
                name: "ContentEntityFileEntity1");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents");

            migrationBuilder.AddColumn<long>(
                name: "ContentEntityId",
                table: "Files",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ContentEntityId1",
                table: "Files",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentEntityLocale",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentEntityLocale1",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ContentEntityId_ContentEntityLocale",
                table: "Files",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_ContentEntityId1_ContentEntityLocale1",
                table: "Files",
                columns: new[] { "ContentEntityId1", "ContentEntityLocale1" });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PhotoId",
                table: "Contents",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Contents_ContentEntityId1_ContentEntityLocale1",
                table: "Files",
                columns: new[] { "ContentEntityId1", "ContentEntityLocale1" },
                principalTable: "Contents",
                principalColumns: new[] { "Id", "Locale" });

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Contents_ContentEntityId_ContentEntityLocale",
                table: "Files",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" },
                principalTable: "Contents",
                principalColumns: new[] { "Id", "Locale" });
        }
    }
}

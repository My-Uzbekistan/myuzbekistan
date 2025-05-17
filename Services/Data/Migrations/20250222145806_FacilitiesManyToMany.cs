using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class FacilitiesManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Contents_ContentEntityId_ContentEntityLocale",
                table: "Facilities");

            migrationBuilder.DropIndex(
                name: "IX_Facilities_ContentEntityId_ContentEntityLocale",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "ContentEntityId",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "ContentEntityLocale",
                table: "Facilities");

            migrationBuilder.CreateTable(
                name: "ContentEntityFacilityEntity",
                columns: table => new
                {
                    ContentsId = table.Column<long>(type: "bigint", nullable: false),
                    ContentsLocale = table.Column<string>(type: "text", nullable: false),
                    FacilitiesId = table.Column<long>(type: "bigint", nullable: false),
                    FacilitiesLocale = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEntityFacilityEntity", x => new { x.ContentsId, x.ContentsLocale, x.FacilitiesId, x.FacilitiesLocale });
                    table.ForeignKey(
                        name: "FK_ContentEntityFacilityEntity_Contents_ContentsId_ContentsLoc~",
                        columns: x => new { x.ContentsId, x.ContentsLocale },
                        principalTable: "Contents",
                        principalColumns: new[] { "Id", "Locale" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentEntityFacilityEntity_Facilities_FacilitiesId_Facilit~",
                        columns: x => new { x.FacilitiesId, x.FacilitiesLocale },
                        principalTable: "Facilities",
                        principalColumns: new[] { "Id", "Locale" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentEntityFacilityEntity_FacilitiesId_FacilitiesLocale",
                table: "ContentEntityFacilityEntity",
                columns: new[] { "FacilitiesId", "FacilitiesLocale" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentEntityFacilityEntity");

            migrationBuilder.AddColumn<long>(
                name: "ContentEntityId",
                table: "Facilities",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentEntityLocale",
                table: "Facilities",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ContentEntityId_ContentEntityLocale",
                table: "Facilities",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" });

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Contents_ContentEntityId_ContentEntityLocale",
                table: "Facilities",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" },
                principalTable: "Contents",
                principalColumns: new[] { "Id", "Locale" });
        }
    }
}

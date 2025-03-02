using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterPrimaryRegionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Regions_RegionId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Regions_ParentRegionId",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ParentRegionId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Contents_RegionId",
                table: "Contents");

            migrationBuilder.AddColumn<string>(
                name: "ParentRegionLocale",
                table: "Regions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionLocale",
                table: "Contents",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                columns: new[] { "Id", "Locale" });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentRegionId_ParentRegionLocale",
                table: "Regions",
                columns: new[] { "ParentRegionId", "ParentRegionLocale" });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_RegionId_RegionLocale",
                table: "Contents",
                columns: new[] { "RegionId", "RegionLocale" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Regions_RegionId_RegionLocale",
                table: "Contents",
                columns: new[] { "RegionId", "RegionLocale" },
                principalTable: "Regions",
                principalColumns: new[] { "Id", "Locale" });

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Regions_ParentRegionId_ParentRegionLocale",
                table: "Regions",
                columns: new[] { "ParentRegionId", "ParentRegionLocale" },
                principalTable: "Regions",
                principalColumns: new[] { "Id", "Locale" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Regions_RegionId_RegionLocale",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Regions_ParentRegionId_ParentRegionLocale",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ParentRegionId_ParentRegionLocale",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Contents_RegionId_RegionLocale",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ParentRegionLocale",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "RegionLocale",
                table: "Contents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentRegionId",
                table: "Regions",
                column: "ParentRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_RegionId",
                table: "Contents",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Regions_RegionId",
                table: "Contents",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Regions_ParentRegionId",
                table: "Regions",
                column: "ParentRegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}

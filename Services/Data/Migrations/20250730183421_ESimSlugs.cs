using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class ESimSlugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM public."ESimOrders";
                DELETE FROM public."PackageDiscounts";
                DELETE FROM public."ESimPackages";
            """);

            migrationBuilder.AddColumn<long>(
                name: "ESimSlugId",
                table: "ESimPackages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasVoicePack",
                table: "ESimPackages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ESimSlugs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    TitleUz = table.Column<string>(type: "text", nullable: false),
                    TitleRu = table.Column<string>(type: "text", nullable: false),
                    TitleEn = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    SlugType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESimSlugs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESimPackages_ESimSlugId",
                table: "ESimPackages",
                column: "ESimSlugId");

            migrationBuilder.AddForeignKey(
                name: "FK_ESimPackages_ESimSlugs_ESimSlugId",
                table: "ESimPackages",
                column: "ESimSlugId",
                principalTable: "ESimSlugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ESimPackages_ESimSlugs_ESimSlugId",
                table: "ESimPackages");

            migrationBuilder.DropTable(
                name: "ESimSlugs");

            migrationBuilder.DropIndex(
                name: "IX_ESimPackages_ESimSlugId",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "ESimSlugId",
                table: "ESimPackages");

            migrationBuilder.DropColumn(
                name: "HasVoicePack",
                table: "ESimPackages");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class EsimPackageDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PackageDiscountId",
                table: "ESimPackages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PackageDiscounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ESimPackageId = table.Column<long>(type: "bigint", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "double precision", nullable: false),
                    DiscountPrice = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageDiscounts_ESimPackages_ESimPackageId",
                        column: x => x.ESimPackageId,
                        principalTable: "ESimPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageDiscounts_ESimPackageId",
                table: "PackageDiscounts",
                column: "ESimPackageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageDiscounts");

            migrationBuilder.DropColumn(
                name: "PackageDiscountId",
                table: "ESimPackages");
        }
    }
}

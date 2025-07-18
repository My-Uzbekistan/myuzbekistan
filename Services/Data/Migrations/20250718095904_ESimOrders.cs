using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class ESimOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESimOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    OrderCode = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    EsimType = table.Column<string>(type: "text", nullable: false),
                    Package = table.Column<string>(type: "text", nullable: false),
                    PackageId = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Validity = table.Column<int>(type: "integer", nullable: false),
                    SimId = table.Column<int>(type: "integer", nullable: false),
                    SimCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Iccid = table.Column<string>(type: "text", nullable: false),
                    Lpa = table.Column<string>(type: "text", nullable: false),
                    MatchingId = table.Column<string>(type: "text", nullable: false),
                    ConfirmationCode = table.Column<string>(type: "text", nullable: true),
                    QrCode = table.Column<string>(type: "text", nullable: false),
                    QrCodeUrl = table.Column<string>(type: "text", nullable: false),
                    DirectAppleUrl = table.Column<string>(type: "text", nullable: false),
                    ManualInstallation = table.Column<string>(type: "text", nullable: false),
                    QrCodeInstallation = table.Column<string>(type: "text", nullable: false),
                    CustomPrice = table.Column<double>(type: "double precision", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "double precision", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESimOrders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESimOrders");
        }
    }
}

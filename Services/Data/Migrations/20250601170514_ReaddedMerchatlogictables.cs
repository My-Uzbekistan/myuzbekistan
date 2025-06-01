using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReaddedMerchatlogictables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Files_ImageId",
                table: "Merchants");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Merchants_ParentId",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Contract",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Inn",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "MXIKCode",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PayDay",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "TypeOfService",
                table: "Merchants");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Merchants",
                newName: "MerchantCategoryId");

            migrationBuilder.RenameColumn(
                name: "IsVat",
                table: "Merchants",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Merchants",
                newName: "LogoId");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Merchants",
                newName: "WorkTime");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_ParentId",
                table: "Merchants",
                newName: "IX_Merchants_MerchantCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_ImageId",
                table: "Merchants",
                newName: "IX_Merchants_LogoId");

            migrationBuilder.CreateTable(
                name: "MerchantCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogoId = table.Column<long>(type: "bigint", nullable: true),
                    BrandName = table.Column<string>(type: "text", nullable: true),
                    OrganizationName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Inn = table.Column<string>(type: "text", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    MfO = table.Column<string>(type: "text", nullable: true),
                    Contract = table.Column<string>(type: "text", nullable: true),
                    Discount = table.Column<short>(type: "smallint", nullable: false),
                    PayDay = table.Column<byte>(type: "smallint", nullable: false),
                    ServiceType = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsVat = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantCategories_Files_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantCategories_LogoId",
                table: "MerchantCategories",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Files_LogoId",
                table: "Merchants",
                column: "LogoId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId",
                table: "Merchants",
                column: "MerchantCategoryId",
                principalTable: "MerchantCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Files_LogoId",
                table: "Merchants");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId",
                table: "Merchants");

            migrationBuilder.DropTable(
                name: "MerchantCategories");

            migrationBuilder.RenameColumn(
                name: "WorkTime",
                table: "Merchants",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Merchants",
                newName: "IsVat");

            migrationBuilder.RenameColumn(
                name: "MerchantCategoryId",
                table: "Merchants",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "LogoId",
                table: "Merchants",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_MerchantCategoryId",
                table: "Merchants",
                newName: "IX_Merchants_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_LogoId",
                table: "Merchants",
                newName: "IX_Merchants_ImageId");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Merchants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contract",
                table: "Merchants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Discount",
                table: "Merchants",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Inn",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Merchants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MXIKCode",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "PayDay",
                table: "Merchants",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfService",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Files_ImageId",
                table: "Merchants",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Merchants_ParentId",
                table: "Merchants",
                column: "ParentId",
                principalTable: "Merchants",
                principalColumn: "Id");
        }
    }
}

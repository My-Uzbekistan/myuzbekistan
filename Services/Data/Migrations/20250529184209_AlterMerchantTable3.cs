using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterMerchantTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentAccount",
                table: "Merchants",
                newName: "TypeOfService");

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
                name: "Responsible",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "MXIKCode",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PayDay",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Responsible",
                table: "Merchants");

            migrationBuilder.RenameColumn(
                name: "TypeOfService",
                table: "Merchants",
                newName: "CurrentAccount");
        }
    }
}

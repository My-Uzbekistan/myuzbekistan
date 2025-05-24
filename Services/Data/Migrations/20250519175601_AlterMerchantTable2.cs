using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterMerchantTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contract",
                table: "Merchants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentAccount",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

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
                name: "IsVat",
                table: "Merchants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Mfi",
                table: "Merchants",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contract",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "CurrentAccount",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Inn",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "IsVat",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Mfi",
                table: "Merchants");
        }
    }
}

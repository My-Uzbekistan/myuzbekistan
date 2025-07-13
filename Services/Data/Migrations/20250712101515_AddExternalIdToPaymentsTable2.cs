using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalIdToPaymentsTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CardId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardId",
                table: "Payments",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_CardId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Payments");
        }
    }
}

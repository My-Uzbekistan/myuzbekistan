using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class EsimOrderPromoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PromoCodeId",
                table: "ESimOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ESimOrders_PromoCodeId",
                table: "ESimOrders",
                column: "PromoCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ESimOrders_ESimPromoCodes_PromoCodeId",
                table: "ESimOrders",
                column: "PromoCodeId",
                principalTable: "ESimPromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ESimOrders_ESimPromoCodes_PromoCodeId",
                table: "ESimOrders");

            migrationBuilder.DropIndex(
                name: "IX_ESimOrders_PromoCodeId",
                table: "ESimOrders");

            migrationBuilder.DropColumn(
                name: "PromoCodeId",
                table: "ESimOrders");
        }
    }
}

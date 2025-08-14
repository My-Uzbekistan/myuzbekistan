using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatusToInvoicesTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Pending = 0, Completed = 1, Failed = 2, Refunded = 3 ",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "  Pending = 0, Completed = 1, Failed = 2, Refunded = 3 ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Invoices",
                type: "integer",
                nullable: true,
                comment: "  Pending = 0, Completed = 1, Failed = 2, Refunded = 3 ",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Pending = 0, Completed = 1, Failed = 2, Refunded = 3 ");
        }
    }
}

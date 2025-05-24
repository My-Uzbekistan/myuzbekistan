using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Date.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddBalanceColumnToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "aspnet",
                table: "AspNetUsers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "aspnet",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeNameToCardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");
        }
    }
}

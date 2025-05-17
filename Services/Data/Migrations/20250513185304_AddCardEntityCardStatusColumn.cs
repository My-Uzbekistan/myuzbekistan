using Microsoft.EntityFrameworkCore.Migrations;
using myuzbekistan.Shared;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardEntityCardStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<CardStatus>(
                name: "CardStatus",
                table: "Cards",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardStatus",
                table: "Cards");
        }
    }
}

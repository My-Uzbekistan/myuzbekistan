using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCardEntityCvvField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cvv",
                table: "Cards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cvv",
                table: "Cards",
                type: "text",
                nullable: true);
        }
    }
}

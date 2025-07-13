using Microsoft.EntityFrameworkCore.Migrations;
using myuzbekistan.Shared;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardStatus",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IsMulticard",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PayerId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SmsInform",
                table: "Cards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedOn",
                table: "Cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Cards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<CardStatus>(
                name: "CardStatus",
                table: "Cards",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMulticard",
                table: "Cards",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerId",
                table: "Cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmsInform",
                table: "Cards",
                type: "boolean",
                nullable: true);
        }
    }
}

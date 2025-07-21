using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColorNameColumnsInCardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardColors_CodeId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CodeId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cards",
                newName: "Icon");

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Cards",
                newName: "Name");

            migrationBuilder.AddColumn<long>(
                name: "CodeId",
                table: "Cards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CodeId",
                table: "Cards",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardColors_CodeId",
                table: "Cards",
                column: "CodeId",
                principalTable: "CardColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

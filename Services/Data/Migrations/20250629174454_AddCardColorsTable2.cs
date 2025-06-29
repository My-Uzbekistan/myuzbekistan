using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardColorsTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
              name: "FK_Cards_CardColors_CodeId",
              table: "Cards",
              column: "CodeId",
              principalTable: "CardColors",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);

            migrationBuilder.InsertData(
                table: "CardColors",
                columns: new[] { "Name", "ColorCode","CreatedAt" },
                values: new object[,]
                {
      { "Green", "#2EA866",DateTime.UtcNow, },
      { "Medium Green", "#3DCC78",DateTime.UtcNow, },
      { "Orange", "#FDA543" , DateTime.UtcNow},
      { "Red Orange", "#FF6848" , DateTime.UtcNow},
      { "Rose Pink", "#F65B71" , DateTime.UtcNow},
      { "Medium Purple", "#A464C4" , DateTime.UtcNow},
      { "Light Purple", "#CB83FC" , DateTime.UtcNow},
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterPrimaryKeyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmsTemplates",
                table: "SmsTemplates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmsTemplates",
                table: "SmsTemplates",
                columns: new[] { "Id", "Locale" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmsTemplates",
                table: "SmsTemplates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmsTemplates",
                table: "SmsTemplates",
                column: "Id");
        }
    }
}

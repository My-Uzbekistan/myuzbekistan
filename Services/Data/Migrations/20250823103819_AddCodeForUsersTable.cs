using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Date.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddCodeForUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "aspnet",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "aspnet",
                table: "AspNetUsers");
        }
    }
}

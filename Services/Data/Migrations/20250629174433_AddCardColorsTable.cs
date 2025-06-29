using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardColorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Cards");

            migrationBuilder.AddColumn<long>(
                name: "CodeId",
                table: "Cards",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CardColors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardColors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CodeId",
                table: "Cards",
                column: "CodeId");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardColors_CodeId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "CardColors");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CodeId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Cards",
                type: "text",
                nullable: true);
        }
    }
}

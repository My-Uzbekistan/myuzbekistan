using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using myuzbekistan.Shared;

#nullable disable

namespace Services.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facilities",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "Contents");

            migrationBuilder.AddColumn<List<CallInformation>>(
                name: "PhoneNumbers",
                table: "Contents",
                type: "jsonb",
                nullable: true,
                defaultValue: new List<CallInformation>());


            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconId = table.Column<long>(type: "bigint", nullable: false),
                    ContentEntityId = table.Column<long>(type: "bigint", nullable: true),
                    ContentEntityLocale = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => new { x.Id, x.Locale });
                    table.ForeignKey(
                        name: "FK_Facilities_Contents_ContentEntityId_ContentEntityLocale",
                        columns: x => new { x.ContentEntityId, x.ContentEntityLocale },
                        principalTable: "Contents",
                        principalColumns: new[] { "Id", "Locale" });
                    table.ForeignKey(
                        name: "FK_Facilities_Files_IconId",
                        column: x => x.IconId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ContentEntityId = table.Column<long>(type: "bigint", nullable: true),
                    ContentEntityLocale = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => new { x.Id, x.Locale });
                    table.ForeignKey(
                        name: "FK_Languages_Contents_ContentEntityId_ContentEntityLocale",
                        columns: x => new { x.ContentEntityId, x.ContentEntityLocale },
                        principalTable: "Contents",
                        principalColumns: new[] { "Id", "Locale" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ContentEntityId_ContentEntityLocale",
                table: "Facilities",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" });

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_IconId",
                table: "Facilities",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ContentEntityId_ContentEntityLocale",
                table: "Languages",
                columns: new[] { "ContentEntityId", "ContentEntityLocale" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.AlterColumn<int[]>(
                name: "PhoneNumbers",
                table: "Contents",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(List<CallInformation>),
                oldType: "jsonb");

            migrationBuilder.AddColumn<string[]>(
                name: "Facilities",
                table: "Contents",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Languages",
                table: "Contents",
                type: "text[]",
                nullable: true);
        }
    }
}

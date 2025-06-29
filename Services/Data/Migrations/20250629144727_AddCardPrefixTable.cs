using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardPrefixTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cvv",
                table: "Cards",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CardPrefixes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Prefix = table.Column<long>(type: "bigint", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: false),
                    CardType = table.Column<string>(type: "text", nullable: false),
                    CardBrandId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrefixes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPrefixes_Files_CardBrandId",
                        column: x => x.CardBrandId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardPrefixes_CardBrandId",
                table: "CardPrefixes",
                column: "CardBrandId");

              migrationBuilder.InsertData(
table: "CardPrefixes",
columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
values: new object?[] {
    "Ziraat Bank",
    860020,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
});
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ziraat Bank",
    986029,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ziraat Bank",
    56146806,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turkistonbank",
    626282,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turkistonbank",
    860038,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turkistonbank",
    986021,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Saderat Bank",
    860043,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Saderat Bank",
    986030,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ravnaq Bank",
    860050,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ravnaq Bank",
    986024,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ravnaq Bank",
    56146815,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ravnaq Bank",
    56146823,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asia Alliance Bank",
    626272,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asia Alliance Bank",
    860055,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asia Alliance Bank",
    986009,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asia Alliance Bank",
    56146826,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asia Alliance Bank",
    56146809,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Orient Finans Bank",
    626255,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Orient Finans Bank",
    860057,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Orient Finans Bank",
    986027,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Orient Finans Bank",
    56146829,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "National Bank",
    626263,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "National Bank",
    860002,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "National Bank",
    986012,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "National Bank",
    56146820,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "National Bank",
    56146814,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "National Bank",
    56146819,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "O`zsanoatqurilishbank",
    860003,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "O`zsanoatqurilishbank",
    986002,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "O`zsanoatqurilishbank",
    56146816,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    62625,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    860004,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    986003,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    56146801,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    56146838,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    407342,
    "Unknown",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Agrobank",
    626257,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Mikrokreditbank",
    860005,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Mikrokreditbank",
    986013,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Mikrokreditbank",
    626292,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Mikrokreditbank",
    56146827,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Mikrokreditbank",
    56146841,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Mikrokreditbank",
    56146813,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Xalq Bank",
    860006,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Xalq Bank",
    986008,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Xalq Bank",
    626291,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Xalq Bank",
    56146802,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Garant Bank",
    860008,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Garant Bank",
    986014,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "BRB bank",
    860009,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "BRB bank",
    986006,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "BRB bank",
    626248,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "BRB bank",
    56146828,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "BRB bank",
    56146804,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turon Bank",
    860011,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turon Bank",
    986015,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turon Bank",
    56146807,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Turon Bank",
    56146885,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Hamkor Bank",
    860012,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Hamkor Bank",
    986016,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Hamkor Bank",
    626256,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Hamkor Bank",
    56146818,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asaka Bank",
    860013,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asaka Bank",
    986004,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asaka Bank",
    56146830,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Asaka Bank",
    544081,
    "Unknown",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipak Yuli Bank",
    860014,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipak Yuli Bank",
    986017,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipak Yuli Bank",
    626249,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipak Yuli Bank",
    56146821,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Trastbank",
    860030,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Trastbank",
    986018,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Trastbank",
    56146824,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    860031,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    986019,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    626247,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    561468,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    56146800,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    56146839,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Aloqabank",
    56146887,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipoteka bank",
    860033,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipoteka bank",
    986001,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Ipoteka bank",
    56146812,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "O`zKDB bank",
    860034,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "O`zKDB bank",
    986020,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "O`zKDB bank",
    56146825,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Universalbank",
    860048,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Universalbank",
    986023,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Universalbank",
    626283,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Universalbank",
    56146840,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Universalbank",
    56146810,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Kapital bank",
    860049,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Kapital bank",
    986010,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Kapital bank",
    56146822,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Davr bank",
    860051,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Davr bank",
    986025,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Davr bank",
    626273,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Davr bank",
    56146861,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Davr bank",
    56146805,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Invest-Finans bank",
    860053,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Invest-Finans bank",
    986026,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Invest-Finans bank",
    626253,
    "UnionPay",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Invest-Finans bank",
    56146831,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Madad Invest Bank",
    860058,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Madad Invest Bank",
    986032,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Madad Invest Bank",
    56146832,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "AVO bank",
    860059,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "AVO bank",
    986031,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "AVO bank",
    56146833,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Poytaxt Bank",
    860060,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Poytaxt Bank",
    986033,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Poytaxt Bank",
    56146808,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Tengebank",
    860061,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Tengebank",
    986034,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Tengebank",
    56146817,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "TBC bank",
    860062,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "TBC bank",
    986035,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "TBC bank",
    56146834,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Anor bank",
    860063,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Anor bank",
    986060,
    "Humo",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Anor bank",
    56146835,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Uzum bank",
    986036,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Uzum bank",
    56146836,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Garant bank",
    56146803,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Hayot bank",
    56146877,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Hayot bank",
    56146888,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Smart Bank",
    56146886,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Smart Bank",
    56146873,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
            migrationBuilder.InsertData(
                table: "CardPrefixes",
                columns: new[] { "BankName", "Prefix", "CardType", "CardBrandId", "CreatedAt", "UpdatedAt" },
                values: new object?[] {
    "Smart Bank",
    56146837,
    "Uzcard",
    null,
    DateTime.UtcNow,
    null
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardPrefixes");

            migrationBuilder.DropColumn(
                name: "Cvv",
                table: "Cards");
        }
    }
}

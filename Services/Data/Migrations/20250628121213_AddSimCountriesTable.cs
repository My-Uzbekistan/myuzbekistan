using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Services.Data.Migrations
{

    /// <inheritdoc />
    public partial class AddSimCountriesTable : Migration
    {
        public string json = """
                [
          {
            "Locale": "en",
            "Name": "Afghanistan",
            "Title": "🇦🇫 Afghanistan",
            "Code": "AF",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Афганистан",
            "Title": "🇦🇫 Афганистан",
            "Code": "AF",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Afg'oniston",
            "Title": "🇦🇫 Afg'oniston",
            "Code": "AF",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Albania",
            "Title": "🇦🇱 Albania",
            "Code": "AL",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Албания",
            "Title": "🇦🇱 Албания",
            "Code": "AL",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Albaniya",
            "Title": "🇦🇱 Albaniya",
            "Code": "AL",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Anguilla",
            "Title": "🇦🇳 Anguilla",
            "Code": "AN",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Anguilla",
            "Title": "🇦🇳 Anguilla",
            "Code": "AN",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Anguilla",
            "Title": "🇦🇳 Anguilla",
            "Code": "AN",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Argentina",
            "Title": "🇦🇷 Argentina",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Аргентина",
            "Title": "🇦🇷 Аргентина",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Argentina",
            "Title": "🇦🇷 Argentina",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Armenia",
            "Title": "🇦🇷 Armenia",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Армения",
            "Title": "🇦🇷 Армения",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Armaniston",
            "Title": "🇦🇷 Armaniston",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Aruba",
            "Title": "🇦🇷 Aruba",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Aruba",
            "Title": "🇦🇷 Aruba",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Aruba",
            "Title": "🇦🇷 Aruba",
            "Code": "AR",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Australia",
            "Title": "🇦🇺 Australia",
            "Code": "AU",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Австралия",
            "Title": "🇦🇺 Австралия",
            "Code": "AU",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Avstraliya",
            "Title": "🇦🇺 Avstraliya",
            "Code": "AU",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Austria",
            "Title": "🇦🇺 Austria",
            "Code": "AU",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Австрия",
            "Title": "🇦🇺 Австрия",
            "Code": "AU",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Avstriya",
            "Title": "🇦🇺 Avstriya",
            "Code": "AU",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Azerbaijan",
            "Title": "🇦🇿 Azerbaijan",
            "Code": "AZ",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Азербайджан",
            "Title": "🇦🇿 Азербайджан",
            "Code": "AZ",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Ozarbayjon",
            "Title": "🇦🇿 Ozarbayjon",
            "Code": "AZ",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Canada",
            "Title": "🇨🇦 Canada",
            "Code": "CA",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Канада",
            "Title": "🇨🇦 Канада",
            "Code": "CA",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Kanada",
            "Title": "🇨🇦 Kanada",
            "Code": "CA",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "China",
            "Title": "🇨🇭 China",
            "Code": "CH",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Китай",
            "Title": "🇨🇭 Китай",
            "Code": "CH",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Xitoy",
            "Title": "🇨🇭 Xitoy",
            "Code": "CH",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "France",
            "Title": "🇫🇷 France",
            "Code": "FR",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Франция",
            "Title": "🇫🇷 Франция",
            "Code": "FR",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Fransiya",
            "Title": "🇫🇷 Fransiya",
            "Code": "FR",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Germany",
            "Title": "🇬🇪 Germany",
            "Code": "GE",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Германия",
            "Title": "🇬🇪 Германия",
            "Code": "GE",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Germaniya",
            "Title": "🇬🇪 Germaniya",
            "Code": "GE",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Greece",
            "Title": "🇬🇷 Greece",
            "Code": "GR",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Greece",
            "Title": "🇬🇷 Greece",
            "Code": "GR",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Greece",
            "Title": "🇬🇷 Greece",
            "Code": "GR",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Hong Kong",
            "Title": "🇭🇴 Hong Kong",
            "Code": "HO",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Hong Kong",
            "Title": "🇭🇴 Hong Kong",
            "Code": "HO",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Hong Kong",
            "Title": "🇭🇴 Hong Kong",
            "Code": "HO",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Japan",
            "Title": "🇯🇦 Japan",
            "Code": "JA",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Япония",
            "Title": "🇯🇦 Япония",
            "Code": "JA",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Yaponiya",
            "Title": "🇯🇦 Yaponiya",
            "Code": "JA",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Malaysia",
            "Title": "🇲🇦 Malaysia",
            "Code": "MA",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Malaysia",
            "Title": "🇲🇦 Malaysia",
            "Code": "MA",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Malaysia",
            "Title": "🇲🇦 Malaysia",
            "Code": "MA",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Mexico",
            "Title": "🇲🇪 Mexico",
            "Code": "ME",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Mexico",
            "Title": "🇲🇪 Mexico",
            "Code": "ME",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Mexico",
            "Title": "🇲🇪 Mexico",
            "Code": "ME",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Singapore",
            "Title": "🇸🇮 Singapore",
            "Code": "SI",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Singapore",
            "Title": "🇸🇮 Singapore",
            "Code": "SI",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Singapore",
            "Title": "🇸🇮 Singapore",
            "Code": "SI",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Spain",
            "Title": "🇸🇵 Spain",
            "Code": "SP",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Испания",
            "Title": "🇸🇵 Испания",
            "Code": "SP",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Ispaniya",
            "Title": "🇸🇵 Ispaniya",
            "Code": "SP",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "Turkey",
            "Title": "🇹🇺 Turkey",
            "Code": "TU",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Турция",
            "Title": "🇹🇺 Турция",
            "Code": "TU",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Turkiya",
            "Title": "🇹🇺 Turkiya",
            "Code": "TU",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "United Arab Emirates",
            "Title": "🇺🇳 United Arab Emirates",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "United Arab Emirates",
            "Title": "🇺🇳 United Arab Emirates",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "United Arab Emirates",
            "Title": "🇺🇳 United Arab Emirates",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "United Kingdom",
            "Title": "🇺🇳 United Kingdom",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "Великобритания",
            "Title": "🇺🇳 Великобритания",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "Buyuk Britaniya",
            "Title": "🇺🇳 Buyuk Britaniya",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "en",
            "Name": "United States",
            "Title": "🇺🇳 United States",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "ru",
            "Name": "США",
            "Title": "🇺🇳 США",
            "Code": "UN",
            "Status": true
          },
          {
            "Locale": "uz",
            "Name": "AQSH",
            "Title": "🇺🇳 AQSH",
            "Code": "UN",
            "Status": true
          }
        ]

        """;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SimCountries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimCountries", x => new { x.Id, x.Locale });
                });

            var countries = System.Text.Json.JsonSerializer.Deserialize<List<SimCountryEntity>>(json)!;
            var now = DateTime.UtcNow;

            /* ───── 3. Seeding через InsertData ───── */
            foreach (var c in countries)
            {
                migrationBuilder.InsertData(
                    table: "SimCountries",
                    columns: new[]
                    {
            "Locale", "Name", "Title", "Code",
            "Status", "CreatedAt", "UpdatedAt"
                    },
                    values: new object?[]
                    {
            c.Locale,
            c.Name,
            c.Title,
            c.Code,
            true,          // Status
            now,           // CreatedAt
            null           // UpdatedAt
                    });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimCountries");
        }
    }
}

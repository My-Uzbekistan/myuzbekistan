using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""

                                TRUNCATE  "Regions" CASCADE ;
                ALTER SEQUENCE "Regions_Id_seq" RESTART WITH 1;
                -- Добавляем Узбекистан и получаем его ID
                WITH uzbekistan AS (
                    INSERT INTO public."Regions" ("Name", "Locale", "CreatedAt", "UpdatedAt")
                    VALUES ('Uzbekistan', 'en', NOW(), NOW())
                    RETURNING "Id"
                )
                -- Добавляем Узбекистан для узбекского и русского языков с таким же ID
                INSERT INTO public."Regions" ("Id", "Name", "Locale", "ParentRegionId", "CreatedAt", "UpdatedAt")
                SELECT "Id", 'Oʻzbekiston', 'uz', NULL::BIGINT, NOW(), NOW() FROM uzbekistan
                UNION ALL
                SELECT "Id", 'Узбекистан', 'ru', NULL::BIGINT, NOW(), NOW() FROM uzbekistan;

                -- Добавляем 12 областей с одинаковыми ID для 3 языков
                WITH uzbekistan_id AS (
                    SELECT "Id"::BIGINT FROM public."Regions" WHERE "Name" = 'Uzbekistan' AND "Locale" = 'en' LIMIT 1
                ), regions_data ("Id", "Name_en", "Name_uz", "Name_ru") AS (
                    VALUES
                    (NEXTVAL('public."Regions_Id_seq"'), 'Andijan Region', 'Andijon viloyati', 'Андижанская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Bukhara Region', 'Buxoro viloyati', 'Бухарская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Jizzakh Region', 'Jizzax viloyati', 'Джизакская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Kashkadarya Region', 'Qashqadaryo viloyati', 'Кашкадарьинская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Navoi Region', 'Navoiy viloyati', 'Навоийская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Namangan Region', 'Namangan viloyati', 'Наманганская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Samarkand Region', 'Samarqand viloyati', 'Самаркандская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Surkhandarya Region', 'Surxondaryo viloyati', 'Сурхандарьинская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Syrdarya Region', 'Sirdaryo viloyati', 'Сырдарьинская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Tashkent Region', 'Toshkent viloyati', 'Ташкентская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Fergana Region', 'Fargʻona viloyati', 'Ферганская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Khorezm Region', 'Xorazm viloyati', 'Хорезмская область'),
                    (NEXTVAL('public."Regions_Id_seq"'), 'Tashkent City', 'Toshkent shahri', 'Город Ташкент')
                )
                -- Вставляем регионы на английском
                INSERT INTO public."Regions" ("Id", "Name", "Locale", "ParentRegionId", "CreatedAt", "UpdatedAt")
                SELECT "Id", "Name_en", 'en', (SELECT "Id" FROM uzbekistan_id)::BIGINT, NOW(), NOW() FROM regions_data
                UNION ALL
                -- Вставляем регионы на узбекском с таким же ID
                SELECT "Id", "Name_uz", 'uz', (SELECT "Id" FROM uzbekistan_id)::BIGINT, NOW(), NOW() FROM regions_data
                UNION ALL
                -- Вставляем регионы на русском с таким же ID
                SELECT "Id", "Name_ru", 'ru', (SELECT "Id" FROM uzbekistan_id)::BIGINT, NOW(), NOW() FROM regions_data;
                

                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

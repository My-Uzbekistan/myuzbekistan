using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class DefaultEsimMerchantData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Insert into ServiceTypes
                INSERT INTO public.""ServiceTypes"" (
                    ""Id"", ""Name"", ""Locale"", ""CreatedAt"", ""UpdatedAt""
                ) VALUES (
                    -777, 'ESim', 'uz', now(), now()
                );

                -- Insert into MerchantCategories
                INSERT INTO public.""MerchantCategories"" (
                    ""Id"", ""Locale"", ""BrandName"", ""OrganizationName"", ""Description"",
                    ""Inn"", ""AccountNumber"", ""MfO"", ""Contract"", ""Discount"", ""PayDay"",
                    ""Phone"", ""Email"", ""Address"", ""ServiceTypeId"", ""ServiceTypeLocale"",
                    ""IsVat"", ""Vat"", ""Status"", ""Token"", ""CreatedAt"", ""UpdatedAt""
                ) VALUES (
                    -777, 'uz', 'ESim', 'ESim xizmatlari', 'ESim xizmatlari',
                    '1234567890', '12345678901234567890', '12345', '12345/2023',
                    0, 1, '+998 90 123 45 67', 'support@myuzb.uz', 'myuzbekistan',
                    -777, 'uz', FALSE, 0, TRUE, 'esim_token', now(), now()
                );

                -- Insert into Merchants
                INSERT INTO public.""Merchants"" (
                    ""Id"", ""Locale"", ""Name"", ""Description"", ""Address"",
                    ""WorkTime"", ""Phone"", ""Responsible"", ""Status"", ""Token"",
                    ""MerchantCategoryId"", ""MerchantCategoryLocale"", ""CreatedAt"", ""UpdatedAt""
                ) VALUES (
                    -777, 'uz', 'ESim', 'ESim xizmatlari', 'myuzbekistan',
                    '24/7', '+998 90 123 45 67', 'myuzbekistan', TRUE, 'esim_token',
                    -777, 'uz', now(), now()
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

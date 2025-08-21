using myuzbekistan.Services;
using myuzbekistan.Shared;
using ActualLab.Fusion;

namespace myuzbekistan.Server;
public static class FusionServerExtension
{
    public static FusionBuilder AddUtcServices(this FusionBuilder fusion)
    {
        fusion.AddService<ICategoryService, CategoryService>();
        fusion.AddService<IContentService, ContentService>();
        fusion.AddService<IFacilityService, FacilityService>();
        fusion.AddService<IFavoriteService, FavoriteService>();
        fusion.AddService<IFileService, FileService>();
        fusion.AddService<ILanguageService, LanguageService>();
        fusion.AddService<IReviewService, ReviewService>();
        fusion.AddService<IAuditLogsService, AuditLogService>();
        fusion.AddService<IUserService, UserService>();
        fusion.AddService<IRegionService, RegionService>();
        fusion.AddService<ICurrencyService, CurrencyService>();
        fusion.AddService<IContentStatisticService, ContentStatisticService>();
        fusion.AddService<ICardService, CardService>();
        fusion.AddService<IPaymentService, PaymentService>();
        fusion.AddService<IRegionService, RegionService>();
        fusion.AddService<IMerchantService, MerchantService>();
        fusion.AddService<IMerchantCategoryService, MerchantCategoryService>();
        fusion.AddService<IWeatherService, WeatherService>();
        fusion.AddService<IServiceTypeService, ServiceTypeService>();
        fusion.AddService<IInvoiceService, InvoiceService>();
        fusion.AddService<ISimCountryService, SimCountryService>();
        fusion.AddService<ICardPrefixService, CardPrefixService>();
        fusion.AddService<ICardColorService, CardColorService>();
        fusion.AddService<IAiraloCountryService, AiraloCountryService>();
        fusion.AddService<IAiraloTokenService, AiraloTokenService>();
        fusion.AddService<IAiraloPackageService, AiraloPackageService>();
        fusion.AddService<IESimPackageService, ESimPackageService>();
        fusion.AddService<IESimOrderService, ESimOrderService>();
        fusion.AddService<IESimSlugService, ESimSlugService>();
        fusion.AddService<IESimPromoCodeService, ESimPromoCodeService>();

        return fusion;
    }
}

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

        return fusion;
    }
}

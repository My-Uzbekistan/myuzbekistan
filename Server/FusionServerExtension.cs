using myuzbekistan.Services;
using myuzbekistan.Shared;
using ActualLab.Fusion;
using Services.Features.User;

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
        
        return fusion;
    }
}

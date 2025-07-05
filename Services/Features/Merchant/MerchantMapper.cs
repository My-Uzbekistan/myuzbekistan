[Mapper]
public static partial class MerchantMapper 
{
    #region Usable
    public static MerchantView MapToView(this MerchantEntity src) => src.To();

    public static MerchantResponse MapToResponse(this MerchantEntity src) => src.ToResponse();
    public static List<MerchantResponse> MapToResponseList(this List<MerchantEntity> src) => src.ToResponseList();
    public static List<MerchantView> MapToViewList(this List<MerchantEntity> src)=> src.ToList();
    public static MerchantEntity MapFromView(this MerchantView src) => src.From();
    #endregion

    #region Internal
    private static  MerchantResponse ToResponse(this MerchantEntity src)
    {
        var target = new global::MerchantResponse
        {
            Id = src.Id,
            Logo = src.Logo?.Path,
            Name = src.Name,
            Description = src.Description,
            Address = src.Address,
            WorkTime = src.WorkTime,
            Phone = src.Phone,
            Type = src.MerchantCategory.ServiceType.Name
        };
        return target;

    }

    private static partial List<MerchantResponse> ToResponseList(this List<MerchantEntity> src);

    [UserMapping(Default = true)]
    [MapProperty("Logo", "LogoView")]
    [MapProperty("MerchantCategory", "MerchantCategoryView")]
    private static partial MerchantView To(this MerchantEntity src);
    private static partial List<MerchantView> ToList(this List<MerchantEntity> src);
    [MapProperty("LogoView", "Logo")]
    [MapProperty("MerchantCategoryView", "MerchantCategory")]
    private static partial MerchantEntity From(this MerchantView MerchantView);
    [MapProperty("LogoView", "Logo")]
    [MapProperty("MerchantCategoryView", "MerchantCategory")]
    public static partial void From(MerchantView personView, MerchantEntity personEntity);

    #endregion
}
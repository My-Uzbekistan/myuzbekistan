[Mapper]
public static partial class ServiceTypeMapper 
{
    #region Usable
    public static ServiceTypeView MapToView(this ServiceTypeEntity src) => src.To();
    public static List<ServiceTypeView> MapToViewList(this List<ServiceTypeEntity> src)=> src.ToList();
    public static ServiceTypeEntity MapFromView(this ServiceTypeView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial ServiceTypeView To(this ServiceTypeEntity src);
    private static partial List<ServiceTypeView> ToList(this List<ServiceTypeEntity> src);
    private static partial ServiceTypeEntity From(this ServiceTypeView ServiceTypeView);
    public static partial void From(ServiceTypeView personView, ServiceTypeEntity personEntity);

    #endregion
}
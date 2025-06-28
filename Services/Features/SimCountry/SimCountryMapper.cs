[Mapper]
public static partial class SimCountryMapper 
{
    #region Usable
    public static SimCountryView MapToView(this SimCountryEntity src) => src.To();
    public static List<SimCountryView> MapToViewList(this List<SimCountryEntity> src)=> src.ToList();
    public static SimCountryEntity MapFromView(this SimCountryView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial SimCountryView To(this SimCountryEntity src);
    private static partial List<SimCountryView> ToList(this List<SimCountryEntity> src);
    private static partial SimCountryEntity From(this SimCountryView SimCountryView);
    public static partial void From(SimCountryView personView, SimCountryEntity personEntity);

    #endregion
}
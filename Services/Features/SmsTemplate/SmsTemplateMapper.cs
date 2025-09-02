[Mapper]
public static partial class SmsTemplateMapper 
{
    #region Usable
    public static SmsTemplateView MapToView(this SmsTemplateEntity src) => src.To();
    public static List<SmsTemplateView> MapToViewList(this List<SmsTemplateEntity> src)=> src.ToList();
    public static SmsTemplateEntity MapFromView(this SmsTemplateView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial SmsTemplateView To(this SmsTemplateEntity src);
    private static partial List<SmsTemplateView> ToList(this List<SmsTemplateEntity> src);
    private static partial SmsTemplateEntity From(this SmsTemplateView SmsTemplateView);
    public static partial void From(SmsTemplateView personView, SmsTemplateEntity personEntity);

    #endregion
}
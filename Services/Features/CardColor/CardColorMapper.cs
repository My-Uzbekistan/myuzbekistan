[Mapper]
public static partial class CardColorMapper 
{
    #region Usable
    public static CardColorView MapToView(this CardColorEntity src) => src.To();
    public static CardColorViewApi MapToApi(this CardColorEntity src) => src.ToApi();
    public static List<CardColorViewApi> MapToApiList(this List<CardColorEntity> src)=> src.ToApiList();
    public static List<CardColorView> MapToViewList(this List<CardColorEntity> src)=> src.ToList();
    public static CardColorEntity MapFromView(this CardColorView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    [MapProperty("Image", "ImageView")]
    private static partial CardColorView To(this CardColorEntity src);
    [UserMapping(Default = true)]
    [MapProperty("Image", "ImageView")]
    private static  CardColorViewApi ToApi(this CardColorEntity src)
    {
        var to = new CardColorViewApi();
        if(src.Image != null)
        {
            to.Image = Constants.MinioPath + src.Image.Path;
        }

        return to;
    }
    private static partial List<CardColorView> ToList(this List<CardColorEntity> src);
    private static partial List<CardColorViewApi> ToApiList(this List<CardColorEntity> src);
    [MapProperty("ImageView", "Image")]
    private static partial CardColorEntity From(this CardColorView CardColorView);
    [MapProperty("ImageView", "Image")]
    public static partial void From(CardColorView personView, CardColorEntity personEntity);

    #endregion
}
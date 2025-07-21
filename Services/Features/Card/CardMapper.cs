namespace myuzbekistan.Services;

[Mapper]
public static partial class CardMapper 
{
    #region Usable
    public static CardView MapToView(this CardEntity src) => src.To();
    public static CardInfo MapToInfo(this CardEntity src) => src.ToInfo();
    public static List<CardInfo> MapToListInfo(this List<CardEntity> src) => src.ToListInfo();
    public static List<CardView> MapToViewList(this List<CardEntity> src)=> src.ToList();
    public static CardEntity MapFromView(this CardView src) => src.From();
    #endregion

    #region Internal


    
    private static partial CardView To(this CardEntity src);
    
    private static partial List<CardView> ToList(this List<CardEntity> src);

    [MapProperty("Code.ColorCode", "ColorCode")]
    [MapProperty("CardPan", "CardNumber")]
    private static partial CardInfo ToInfo(this CardEntity src);
    private static partial List<CardInfo> ToListInfo(this List<CardEntity> src);
    private static partial CardEntity From(this CardView CardView);
    public static partial void From(CardView personView, CardEntity personEntity);

    #endregion
}
[Mapper]
public static partial class CardColorMapper 
{
    #region Usable
    public static CardColorView MapToView(this CardColorEntity src) => src.To();
    public static List<CardColorView> MapToViewList(this List<CardColorEntity> src)=> src.ToList();
    public static CardColorEntity MapFromView(this CardColorView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial CardColorView To(this CardColorEntity src);
    private static partial List<CardColorView> ToList(this List<CardColorEntity> src);
    private static partial CardColorEntity From(this CardColorView CardColorView);
    public static partial void From(CardColorView personView, CardColorEntity personEntity);

    #endregion
}
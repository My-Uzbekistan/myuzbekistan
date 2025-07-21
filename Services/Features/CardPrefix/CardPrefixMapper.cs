[Mapper]
public static partial class CardPrefixMapper 
{
    #region Usable
    public static CardPrefixView MapToView(this CardPrefixEntity src) => src.To();
    public static List<CardPrefixView> MapToViewList(this List<CardPrefixEntity> src)=> src.ToList();
    public static CardPrefixEntity MapFromView(this CardPrefixView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial CardPrefixView To(this CardPrefixEntity src);
    private static partial List<CardPrefixView> ToList(this List<CardPrefixEntity> src);
    private static partial CardPrefixEntity From(this CardPrefixView CardPrefixView);
    public static partial void From(CardPrefixView personView, CardPrefixEntity personEntity);

    #endregion
}
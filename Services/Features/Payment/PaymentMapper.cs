using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class PaymentMapper 
{
    #region Usable
    public static PaymentView MapToView(this PaymentEntity src) => src.To();
    public static List<PaymentView> MapToViewList(this List<PaymentEntity> src)=> src.ToList();
    public static PaymentEntity MapFromView(this PaymentView src) => src.From();
    #endregion

    #region Internal

    private static partial PaymentView To(this PaymentEntity src);
    private static partial List<PaymentView> ToList(this List<PaymentEntity> src);
    private static partial PaymentEntity From(this PaymentView PaymentView);
    public static partial void From(PaymentView personView, PaymentEntity personEntity);

    #endregion
}
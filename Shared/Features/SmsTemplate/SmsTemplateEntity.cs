using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(SmsTemplateEntity.Id), nameof(SmsTemplateEntity.Locale))]
public class SmsTemplateEntity : BaseEntity
{
    public string Locale { get; set; } = null!;
    public string Template { get; set; } = null!;
    public string Key { get; set; } = null!;
}

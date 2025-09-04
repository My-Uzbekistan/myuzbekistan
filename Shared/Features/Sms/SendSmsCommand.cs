using System.Reactive;
using System.Runtime.Serialization;
using MemoryPack;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record SendSmsCommand(
    [property: DataMember] Session Session,
    [property: DataMember] long TemplateId,
    [property: DataMember] string Locale,
    [property: DataMember] string Phone,
    [property: DataMember] Dictionary<string,string>? Parameters
) : ISessionCommand<Unit>;

public static class SmsTemplatePlaceholders
{
    public const string Code = "%w"; // placeholder for code / dynamic value

    public static string Apply(string template, Dictionary<string,string>? parameters)
    {
        if (string.IsNullOrEmpty(template) || parameters == null) return template;
        if (parameters.TryGetValue("code", out var code) && !string.IsNullOrEmpty(code))
            template = template.Replace(Code, code);
        return template;
    }
}

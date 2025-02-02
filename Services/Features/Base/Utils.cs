using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace myuzbekistan.Services;

public static class Utils
{
    // Get the display text for an enum value:
    // - Use the DisplayAttribute if set on the enum member, so this support localization
    // - Fallback on Humanizer to decamelize the enum member name
    public static string GetDisplayName(this object value)
    {
        // Read the Display attribute name
        var member = value.GetType().GetMember(value.ToString()!)[0];
        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
            return displayAttribute.GetName()!;

        return value!.ToString()!;
    }


}
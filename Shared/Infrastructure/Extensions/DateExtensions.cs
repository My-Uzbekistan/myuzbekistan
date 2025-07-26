using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public static class DateExtensions
{
    public static string FormatThousands(this int number)
        => number.ToString("N0").Replace(", ", " ").Replace(",", " ");
    public static DateTime EndOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }
    public static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
    }

    public static long ToUnixTimeStamp(this DateTime dateTime)
    {
        // Ensure the DateTime is in UTC
        DateTime utcDateTime = dateTime.ToUniversalTime();

        // Calculate Unix timestamp
        DateTime unixEpoch = new(1970, 1, 1);
        return (long)(utcDateTime - unixEpoch).TotalSeconds;
    }
}

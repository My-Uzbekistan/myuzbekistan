using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class CardHelper
{
    public static string GetCardBrandImage(string? cardType)
    {
        return cardType switch
        {
            "Humo" => "https://minio.uzdc.uz/myzubekistan/Images/humo.png",
            "Visa" => "https://minio.uzdc.uz/myzubekistan/Images/visa.png",
            "MasterCard" => "https://minio.uzdc.uz/myzubekistan/Images/master.png",
            "Uzcard" => "https://minio.uzdc.uz/myzubekistan/Images/uzcard.png",
            _ => "https://minio.uzdc.uz/myzubekistan/Images/default.png"
        };
    }
}

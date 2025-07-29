using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class CardHelper
{
    public static string GetCardBrandImage(string cardType = "")
    {
        return cardType.ToLower() switch
        {
            "humo" => "https://minio.uzdc.uz/myzubekistan/Images/humo.png",
            "visa" => "https://minio.uzdc.uz/myzubekistan/Images/visa.png",
            "mastercard" => "https://minio.uzdc.uz/myzubekistan/Images/master.png",
            "uzcard" => "https://minio.uzdc.uz/myzubekistan/Images/uzcard.png",
            _ => "https://minio.uzdc.uz/myzubekistan/Images/default.png"
        };
    }
}

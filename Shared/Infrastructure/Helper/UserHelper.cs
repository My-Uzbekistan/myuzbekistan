using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public static class UserHelper
{
    public static long Id(this ClaimsPrincipal User)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = long.Parse(userIdClaim ?? "0");
        return userId;
    }
}

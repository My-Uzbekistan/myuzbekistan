using ActualLab.Fusion;
using System.Security.Claims;

namespace myuzbekistan.Shared;
public class UserContext
{
    public IEnumerable<Claim> UserClaims = new List<Claim>();
    public Session Session = Session.Default;
}

using tusdotnet.Models.Configuration;
using UFile.Server;

namespace Server;
public class OnAuthorizeEvent : IOnAuthorizeEvent
{
    public Task InvokeAsync(AuthorizeContext ctx)
    {
        return Task.CompletedTask;
    }
    
}

using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using UFile.Server;

namespace Server;
public class OnFileCompleteEvent : IOnFileCompleteEvent
{
    public async Task InvokeAsync(FileCompleteContext ctx)
    {
        ITusFile file = await ctx.GetFileAsync();
        //Dictionary<string, Metadata> metadata = await file.GetMetadataAsync(ctx.CancellationToken);
        Stream content = await file.GetContentAsync(ctx.CancellationToken);
    }
}

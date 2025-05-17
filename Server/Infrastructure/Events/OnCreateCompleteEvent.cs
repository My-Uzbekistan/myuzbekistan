using ActualLab.CommandR;
using UFile.Server;
using UFile.Shared;
using myuzbekistan.Shared;
using tusdotnet.Interfaces;
using Client.Core.Layout;
using tusdotnet.Models;

namespace Server;
public class OnCreateCompleteEvent : IOnCreateCompleteEvent
{
    private readonly ICommander _commander;
    private readonly UserContext _userContext;
    
    public OnCreateCompleteEvent(ICommander commander,UserContext userContext)
    {
        _commander = commander;
        _userContext = userContext;
    }
    public async Task InvokeAsync(UCreateCompleteContext ctx)
    {
        string fileName = ctx.Metadata!.GetMetadataValue("fileName");
  
        FileView fileView = new()
        {
            Name = fileName,
            Path = ctx.FilePath,
            Extension = fileName.Split(".").Last(),
            Size = ctx.FileSize,
            FileId = Guid.Parse(ctx.FileId),
        };
        string[] imgFormat = { "png", "jpg", "jpeg", "svg" };
        if (imgFormat.Contains(fileView.Extension) && fileView.Extension != null)
        {
            fileView.Type = UFileTypes.Image;
        }
        await _commander.Call(new CreateFileCommand(_userContext.Session,fileView));
    }
}
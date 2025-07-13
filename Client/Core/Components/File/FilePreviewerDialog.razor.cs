using MudBlazor;

namespace Client.Core.Components.File;

public partial class FilePreviewerDialog
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public FileView File { get; set; } = null!;
    [Parameter] public string Height { get; set; } = "80vh";
    [Inject] private IUFileService UFileService { get; set; } = null!;
    private string FileUrl = string.Empty;

    private void Close() => MudDialog.Close();

    protected override async Task OnInitializedAsync()
    {
        FileUrl = await GetUrl();
        await base.OnInitializedAsync();
    }

    private async Task<string> GetUrl()
    {
        var file = await UFileService.GetFileUrl();
        return file + File.Path;
    }
}
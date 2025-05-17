using ActualLab.Fusion.UI;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Core.Services;

public class UInjector(UICommander UICommander,
                       Session Session,
                       NavigationManager NavigationManager,
                       ISnackbar Snackbar,
                       PageHistoryState PageHistoryState,
                       IDialogService DialogService)
{
    public UICommander Commander { get; set; } = UICommander;
    public Session Session { get; set; } = Session;
    public NavigationManager NavigationManager { get; set; } = NavigationManager;
    public ISnackbar Snackbar { get; set; } = Snackbar;
    public PageHistoryState PageHistoryState { get; set; } = PageHistoryState;
    public IDialogService DialogService { get; set; } = DialogService;
    public Exception? Exception { get; set; }
    public string BackUrl { get; set; } = string.Empty;

}

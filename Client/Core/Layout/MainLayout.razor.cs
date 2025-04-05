using ActualLab.Fusion.Blazor;
using Client.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using System.Globalization;
using System.Security.Claims;

namespace Client.Core.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [CascadingParameter(Name = "IsDark")]
    public bool IsDark { get; set; }

    [CascadingParameter(Name = "IsDark")]
    public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
    bool _drawerOpen = true;

    [Inject] private LayoutService LayoutService { get; set; } = null!;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override void OnInitialized()
    {
        LayoutService.IsDarkMode = IsDark;
        LayoutService.Updated += OnLayoutServiceUpdated;
        Navigation.LocationChanged += OnLocationChanged;
        UIActionFailureTracker.Changed += OnUIActionFailureTrackerChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider
           .GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            Claims = user.Claims;
        }
        await base.OnInitializedAsync();
    }
    private void OnLayoutServiceUpdated(object? sender, EventArgs e) => StateHasChanged();

    private readonly CultureInfo[] supportedCultures = [
        new CultureInfo("en-US"),
        new CultureInfo("ru-RU"),
        new CultureInfo("uz-Latn"),
    ];

    private CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture == value)
            {
                return;
            }
            var uri = new Uri(Navigation.Uri)
                .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
            var cultureEscaped = Uri.EscapeDataString(value.Name);
            var uriEscaped = Uri.EscapeDataString(uri);
            JSRuntime.InvokeVoidAsyncIgnoreErrors("blazorCulture.set", value.Name);
            Navigation.NavigateTo(
                $"Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}",
                forceLoad: true);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Navigation.LocationChanged -= OnLocationChanged;
        UIActionFailureTracker.Changed -= OnUIActionFailureTrackerChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        => UIActionFailureTracker.Clear();

    private void OnUIActionFailureTrackerChanged()
        => this.NotifyStateHasChanged();

    protected async void HandleLogout()
    {
        await JSRuntime.InvokeVoidAsync("BlazorHelpers.RedirectTo", "/Logout");
    }

}
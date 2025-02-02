using ActualLab.Fusion;
using ActualLab.Fusion.Authentication;
using ActualLab.Fusion.Extensions;
using Client.Core.Layout;
using MudBlazor;

namespace Client.Core.Services;

public class LayoutService(Session session, IAuth auth, ISandboxedKeyValueStore store)
{
    public bool IsDarkMode { get; set; } = false;
    public MudTheme CurrentTheme { get; set; } = Theme.UtcTheme();

    public void SetDarkMode(bool value)
    {
        _ = Task.Run(async () =>
        {
            var user = await auth.GetUser(session);
            if (user != null)
            {
                await store.Set(session, $"@user/{user.Id}/theme", value.ToString());
            }
        });

        IsDarkMode = value;
    }

    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        SetDarkMode(IsDarkMode);
        OnUpdated();
    }

    public event EventHandler Updated = null!;

    private void OnUpdated() => Updated?.Invoke(this, EventArgs.Empty);

}

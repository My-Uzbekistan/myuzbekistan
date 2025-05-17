using System.Drawing;
using MudBlazor;

namespace Client.Core.Layout;

public static class Theme
{
    public static MudTheme UtcTheme()
    {
        var theme = new MudTheme()
        {
            PaletteLight = UtcLightPalette,
            PaletteDark = UtcDarkPalette,
            LayoutProperties = new LayoutProperties()
        };
        return theme;
    }

    #region Utc

    private static readonly PaletteLight UtcLightPalette = new()
    {
        Primary = "#4471AB",
        Black = "#110e2d",
        AppbarText = "#424242",
        AppbarBackground = "rgba(255,255,255,0.8)",
        DrawerBackground = "#ffffff",
        GrayLight = "#e8e8e8",
        GrayLighter = "#f9f9f9",
        Background = "#F0F0F5",
        LinesInputs = "#dfdfdf",
        TertiaryLighten = "#b5cdc5"
    };

    private static readonly PaletteDark UtcDarkPalette = new()
    {
        // Primary = "#7e6fff",
        Primary = "#90CAF9",
        Secondary = "#373941",
        Surface = "#202128",
        Background = "#0F0F12",
        BackgroundGray = "#ffffff",
        AppbarText = "#92929f",
        AppbarBackground = "#202128ff",
        DrawerBackground = "#202128ff",
        ActionDefault = "#74718e",
        ActionDisabled = "#9999994d",
        PrimaryContrastText = "#0F0F12",
        ActionDisabledBackground = "#605f6d4d",
        TextPrimary = "#b2b0bf",
        TextSecondary = "#92929f",
        TextDisabled = "#ffffff33",
        DrawerIcon = "#92929f",
        DrawerText = "#92929f",
        GrayLight = "#2a2833",
        GrayLighter = "#1e1e2d",
        Info = "#4a86ff",
        Success = "#3dcb6c",
        Warning = "#ffb545",
        Error = "#ff3f5f",
        LinesDefault = "#33323e",
        TableLines = "#33323e",
        Divider = "#292838",
        OverlayLight = "#1e1e2d80",
        TableHover = "#37373E",
        LinesInputs = "#3b3b46a3",
        TertiaryLighten = "#273430"

    };
    #endregion



}


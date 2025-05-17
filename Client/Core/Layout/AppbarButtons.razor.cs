// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Client.Core.Services;
using Microsoft.AspNetCore.Components;

namespace Client.Core.Layout;

public partial class AppbarButtons
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

}

using Microsoft.AspNetCore.Components;
using ActualLab.Fusion;
using myuzbekistan.Shared;

namespace Client.Core.Services;

public class PageHistoryState(NavigationManager navManager)
{
    private List<string> previousPages = [];
    public NavigationManager NavManager { get; } = navManager;

    public void AddPageToHistory(string pageName)
    {
        previousPages.Add(pageName);
    }

    public string GetPreviousPage()
        => previousPages.Any() && previousPages.Count > 1 ? previousPages[^2] : "/";

    public string GetGoBackPage()
    {
        if (previousPages.Count > 1)
        {
            // You add a page on initialization, so you need to return the 2nd from the last
            return previousPages.ElementAt(previousPages.Count - 1);
        }

        // Can't go back because you didn't navigate enough
        return previousPages.First();
    }

    public bool CanGoBack()
    {
        return previousPages.Count > 1;
    }

    public void Back(string route)
    {
        if (CanGoBack())
        {
            string prevPage = GetGoBackPage();
            NavManager.NavigateTo(prevPage);
        }
        else
        {
            NavManager.NavigateTo($"/{route}");
        }
    }

    public void SetPage(IMutableState<TableOptions> MutableState, bool isOnParametrSet = false)
    {
        var queryParams = new Dictionary<string, object?>
        {
            ["page"] = MutableState.Value.Page.ToString(),
            ["search"] = string.Empty == MutableState.Value.Search ? null : MutableState.Value.Search
        };

        var newUri = NavManager.GetUriWithQueryParameters(queryParams);
        AddPageToHistory(newUri);

        NavManager.NavigateTo(newUri);

    }
}

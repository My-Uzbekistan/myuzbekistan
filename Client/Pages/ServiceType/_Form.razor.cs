namespace Client.Pages.ServiceType;

public partial class _Form
{
    string Language { get; set; } = CultureInfo.CurrentCulture.Name.Split("-").FirstOrDefault("uz");

    [Parameter]
    public List<ServiceTypeView> Model { get; set; } = [new ServiceTypeView { Locale = "uz" }, new ServiceTypeView { Locale = "ru" }, new ServiceTypeView { Locale = "en" },];
    private Dictionary<string, EditContext?> _contexts = new() { { "uz", null }, { "en", null }, { "ru", null } };
    private Dictionary<string, bool> Errors { get; set; } = new() { { "en", false }, { "uz", false }, { "ru", false } };

    [Inject] UInjector Injector { get; set; } = null!;

    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<List<ServiceTypeView>> OnSubmit { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        foreach (var item in Model)
        {
            _contexts[item.Locale] = new(item);
        }
    }
    void HandleSubmit(EditContext ctx)
    {
        foreach (var item in _contexts)
        {
            if (item.Value!.Validate() || item.Key == Language)
                Errors[item.Key] = false;
            else
                Errors[item.Key] = true;
        }
        if (Errors.All(x => !x.Value))
        {
            OnSubmit.InvokeAsync(Model);
        }
    }

    private string GetTitle() => IsNew ? L["Create"] : L["Edit"] ;
}
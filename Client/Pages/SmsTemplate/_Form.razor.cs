namespace Client.Pages.SmsTemplate;

public partial class _Form
{

    string Language { get; set; } = CultureInfo.CurrentCulture.Name.Split("-").FirstOrDefault("uz");
    [Inject] UInjector Injector { get; set; } = null!;

    [Parameter]
    public List<SmsTemplateView> Model { get; set; } =
        [new SmsTemplateView { Locale = "uz" }, new SmsTemplateView { Locale = "ru" }, new SmsTemplateView { Locale = "en" },];

    private Dictionary<string, EditContext?> _contexts = new() { { "uz", null }, { "en", null }, { "ru", null } };
    private Dictionary<string, bool> Errors { get; set; } = new() { { "en", false }, { "uz", false }, { "ru", false } };

    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<List<SmsTemplateView>> OnSubmit { get; set; }

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
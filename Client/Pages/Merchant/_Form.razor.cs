using Client.Core.Layout;


namespace Client.Pages.Merchant;


public partial class _Form
{
    [Parameter]
    public long MerchantCategoryId { get; set; }
    private bool isLoading = true;
    string Language { get; set; } = CultureInfo.CurrentCulture.Name.Split("-").FirstOrDefault("uz");
    [Parameter]
    public List<MerchantView> Model { get; set; } = [new MerchantView { Locale = "uz" }, new MerchantView { Locale = "ru" }, new MerchantView { Locale = "en" },];
    private Dictionary<string, EditContext?> _contexts = new() { { "uz", null }, { "en", null }, { "ru", null } };
    private Dictionary<string, bool> Errors { get; set; } = new() { { "en", false }, { "uz", false }, { "ru", false } };

    [Inject] UInjector Injector { get; set; } = null!;


    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<List<MerchantView>> OnSubmit { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    protected override void OnInitialized()
    {
        
        foreach (var item in Model)
        {
            _contexts[item.Locale] = new EditContext(item);
        }
        base.OnInitialized();
    }

    private void OnValidSubmit(EditContext context)
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

    private string GetTitle() => IsNew ? L["Create"] : L["Edit"];
}
namespace Client.Pages.AppUserEsim;

public partial class AppUserEsimList
{
    [Parameter] public List<EsimView> EsimViews { get; set; } = [];
    [Inject] UInjector Injector { get; set; } = null!;
}
using System.Text.Json;

namespace Server.Controllers;

[Route("api/esim")]
[ApiController]
[Authorize]
public class EsimController(
    IESimPackageService eSimPackageService,
    IESimOrderService esimOrderService,
    ISessionResolver sessionResolver,
    IESimSlugService esimSlugService,
    ICommander commander) : ControllerBase
{
    [HttpGet("countries")]
    public async Task<IActionResult> GetAllAsync([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
    {
        var countries = await esimSlugService.GetAllCountries(options, cancellationToken);
        return Ok(countries);
    }

    [HttpGet("countries/popular")]
    public async Task<IActionResult> GetPopularAsync([FromQuery] string language, CancellationToken cancellationToken = default)
    {
        var popularCountries = await esimSlugService.GetAllPopularCountries(language.ConvertToLanguage(), cancellationToken);
        return Ok(popularCountries);
    }

    [HttpGet("regions")]
    public async Task<IActionResult> GetRegionsAsync([FromQuery] string language, CancellationToken cancellationToken = default)
    {
        var countries = await esimSlugService.GetAllRegions(language.ConvertToLanguage(), cancellationToken);
        return Ok(countries);
    }

    [HttpGet("plans/local")]
    public async Task<IActionResult> GetLocalPlanAsync([FromQuery] string? countrySlug, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.GetAll(
            new TableOptions() { CountrySlug = countrySlug, SlugType = ESimSlugType.Local }, cancellationToken);
        return Ok(countries);
    }

    [HttpGet("plans/regional")]
    public async Task<IActionResult> GetPlansRegionalAsync([FromQuery] string? countrySlug, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.GetClientViewAll(
            new TableOptions() { CountrySlug = countrySlug, SlugType = ESimSlugType.Regional }, cancellationToken);
        return Ok(countries);
    }

    [HttpGet("plans/global")]
    public async Task<IActionResult> GetPlansGlobalAsync([FromQuery] bool hasVoicePack, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.GetAll(
            new TableOptions() { CountrySlug = "world", SlugType = ESimSlugType.Global, HasVoicePack = hasVoicePack, }, cancellationToken);
        return Ok(countries);
    }
    
    [HttpGet("plans/{id}")]
    public async Task<IActionResult> GetPlansAsync(long id, [FromQuery] string? lang, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.GetClientView(id, lang.ConvertToLanguage(), cancellationToken);
        return Ok(countries);
    }
    
    [HttpGet("plans/{id}/coverages")]
    public async Task<IActionResult> GetPlanCoveragesAsync(long id, [FromQuery] string? lang, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.GetPackageCoverages(id, lang.ConvertToLanguage(), cancellationToken);
        return Ok(countries);
    }


    #region MyEsims

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var orders = await esimOrderService.GetAllEsim(options, session, cancellationToken);
        if (orders is null || orders.TotalItems == 0)
        {
            return NoContent();
        }

        return Ok(orders);
    }

    [HttpGet("my/{id}")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var orders = await esimOrderService.GetEsim(id, session, cancellationToken);
        return Ok(orders);
    }

    [HttpGet("my/details/{id}")]
    public async Task<IActionResult> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var order = await esimOrderService.Get(id, session, cancellationToken);
        return Ok(order);
    }

    [HttpGet("my/installation/{iccid}")]
    public async Task<IActionResult> GetInstallation(string iccid, [FromQuery] string lang, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var order = await eSimPackageService.GetInstallationGuide(iccid, lang.ConvertToLanguage(), session, cancellationToken);
        //return string as json
        return new ContentResult
        {
            Content = order,
            ContentType = "application/json",
            StatusCode = 200
        };
    }

    [HttpPost("order")]
    public async Task<IActionResult> MakeOrder([FromBody] CreateESimOrderView view, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var orderView = await commander.Call(new MakeESimOrderCommand(session, view.PackageId), cancellationToken);
        var esimView = await esimOrderService.GetEsim(orderView.Id, session, cancellationToken);
        return Ok(esimView);
    }

    #endregion
}
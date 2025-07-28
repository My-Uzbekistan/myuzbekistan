using DocumentFormat.OpenXml.Office2010.Excel;

namespace Server.Controllers;

[Route("api/esim")]
[ApiController]
[Authorize]
public class EsimController(IAiraloCountryService airaloCountryService,
    IESimPackageService eSimPackageService,
    IESimOrderService esimOrderService,
    ISessionResolver sessionResolver,
    ICommander commander) : ControllerBase
{
    [HttpGet("countries/{language}")]
    public async Task<IActionResult> GetAllAsync(string language, CancellationToken cancellationToken = default)
    {
        var countries = await airaloCountryService.GetAllAsync(language.ConvertToLanguage(), cancellationToken);
        return Ok(countries);
    }

    [HttpGet("countries/{language}/popular")]
    public async Task<IActionResult> GetPopularAsync(string language, CancellationToken cancellationToken = default)
    {
        var popularCountries = await airaloCountryService.GetPopularAsync(language.ConvertToLanguage(), cancellationToken);
        return Ok(popularCountries);
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetPlansAsync([FromQuery] string? countrySlug, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.GetAll(new TableOptions() { CountrySlug = countrySlug }, cancellationToken);
        return Ok(countries);
    }
    
    [HttpGet("plans/{id}")]
    public async Task<IActionResult> GetPlansAsync(long id, CancellationToken cancellationToken = default)
    {
        var countries = await eSimPackageService.Get(id, cancellationToken);
        return Ok(countries);
    }

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
        using var httpClient = new HttpClient();
        var qrCodeBytes = await httpClient.GetByteArrayAsync(order.QrCode, cancellationToken);
        var base64 = Convert.ToBase64String(qrCodeBytes);

        var data = new
        {
            iccid = order.Iccid,
            smdpAddress = order.Lpa,
            activationCode = order.QrCode,
            qrCodeImageBase64 = base64
        };

        return Ok(order);
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
}
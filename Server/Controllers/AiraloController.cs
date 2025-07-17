using ActualLab.CommandR;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api")]
[ApiController]
public class AiraloController(IAiraloCountryService airaloCountryService,
    IAiraloPackageService airaloPackageService,
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
        var countries = await airaloPackageService.GetCountryPackagesAsync(countrySlug, cancellationToken);
        return Ok(countries);
    }

    [HttpGet("sync")]
    public async Task<IActionResult> SyncAsync(CancellationToken cancellationToken = default)
    {
        await commander.Call(new SyncESimPackagesCommand(), cancellationToken);
        return Ok(new { Message = "Sync completed!" });
    }
}
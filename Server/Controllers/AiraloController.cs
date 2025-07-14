using ActualLab.CommandR;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AiraloController(IAiraloCountryService airaloCountryService,
    ICommander commander) : ControllerBase
{
    [HttpGet("{language}/countries")]
    public async Task<IActionResult> GetAllAsync(string language, CancellationToken cancellationToken = default)
    {
        var countries = await airaloCountryService.GetAllAsync(language.ConvertToLanguage(), cancellationToken);
        return Ok(countries);
    }

    [HttpGet("{language}/countries/popular")]
    public async Task<IActionResult> GetPopularAsync(string language, CancellationToken cancellationToken = default)
    {
        var popularCountries = await airaloCountryService.GetPopularAsync(language.ConvertToLanguage(), cancellationToken);
        return Ok(popularCountries);
    }

    [HttpGet("sync")]
    public async Task<IActionResult> SyncAsync(CancellationToken cancellationToken = default)
    {
        await commander.Call(new SyncESimPackagesCommand(), cancellationToken);
        return Ok(new { Message = "Sync completed!" });
    }
}
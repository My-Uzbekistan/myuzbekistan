using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController(IWeatherService weatherService) : ControllerBase
    {

        [HttpGet]
        public async Task<WeatherView> GetWeather([FromQuery] WeatherRequest request, CancellationToken cancellationToken)
        {
            request.Lang ??= LangHelper.currentLocale;
            return await weatherService.GetWeather(request.Lat, request.Lon, request.Lang, cancellationToken);
        }

        [HttpGet("region")]
        public async Task<WeatherView> GetWeatherByRegionId([FromQuery] WeatherByRegionRequest request, CancellationToken cancellationToken)
        {
            request.Lang ??= LangHelper.currentLocale;
            return await weatherService.GetWeatherByRegion(request.RegionId, request.Lang, cancellationToken);
        }
    }
}

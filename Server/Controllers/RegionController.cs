using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/regions")]
[ApiController]
public class RegionController(IRegionService regionService) : ControllerBase
{

    [HttpGet]
    public async Task<List<RegionApi>> GetRegions(CancellationToken cancellationToken)
    {
        return await regionService.GetRegions(cancellationToken);
    }

}


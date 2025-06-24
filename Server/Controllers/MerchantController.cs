using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;
using Stl.Fusion.Authentication;

namespace Server.Controllers
{
    [Route("api/merchants")]
    [ApiController]

    public class MerchantController(IMerchantService _merchantService) : ControllerBase
    {

        [HttpGet("{id}")]
        public Task<List<MerchantView>> Get(long Id, CancellationToken cancellationToken = default)
        {
            return _merchantService.Get(Id, cancellationToken);
        }
        [HttpGet]
        public Task<TableResponse<MerchantView>> GetAllByApi([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return _merchantService.GetAllByApi( options, cancellationToken);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers
{
    [Route("api/merchants")]
    [ApiController]

    public class MerchantController(IMerchantService _merchantService) : ControllerBase
    {

        [HttpGet("{id}")]
        public Task<MerchantResponse> Get(long Id, CancellationToken cancellationToken = default)
        {
            return _merchantService.GetByApi(Id, cancellationToken);
        }
        [HttpGet]
        public Task<TableResponse<MerchantResponse>> GetAllByApi([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            options.Lang = LangHelper.currentLocale;
            return _merchantService.GetAllByApi( options, cancellationToken);
        }

        [HttpGet("grouped-by-service-type")]
        public Task<List<MerchantsByServiceTypeResponse>> GetGroupedByServiceType([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            options.Lang = options.Lang ?? LangHelper.currentLocale;
            return _merchantService.GetAllGroupedByServiceType(options, cancellationToken);
        }
    }
}

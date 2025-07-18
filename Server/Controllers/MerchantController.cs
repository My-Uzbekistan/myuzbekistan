﻿using Microsoft.AspNetCore.Http;
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
            return _merchantService.GetAllByApi( options, cancellationToken);
        }
    }
}

using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Features.Contributer;
using DesignSphere.Application.Features.Project;
using EventBus.Messages.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SieveCustom.Models;
using System.Net;

namespace DesignSphere.Api.Controllers.v1.Contributer;


[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/contributer")]
public class ContributerController : BaseApiController
{


    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Ulid), (int)HttpStatusCode.OK)]
    public async Task<Ulid> AddContributer([FromBody] AddProjectContributerRequest query)
    {

        return await Mediator.Send(query);
    }

    [HttpGet("GetProjectContributers")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResponseOffset<GetContributerQueryResult>), (int)HttpStatusCode.OK)]
    public async Task<PagedResponseOffset<GetContributerQueryResult>> GetProjectContributers([FromQuery] GetContributerQueryRequest request)
    {
        return await Mediator.Send(request);
    }
}

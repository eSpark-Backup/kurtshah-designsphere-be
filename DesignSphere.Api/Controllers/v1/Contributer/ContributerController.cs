using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Features.Contributer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> AddContributer([FromBody] AddProjectContributerRequest query)
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

    [HttpPost("SendRequestEmailContributers")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> SendRequestEmailContributers([FromBody] SendEmailRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet("IsContributerExist")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<bool> IsContributerExist([FromBody] IsContributerExistRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("RemoveProjectContributer")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> RemoveProjectContributer([FromQuery] RemoveProjectContributerRequest request)
    {
        return await Mediator.Send(request);
    }
}

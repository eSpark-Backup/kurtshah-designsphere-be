using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Features.Project;
using EventBus.Messages.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SieveCustom.Models;
using System.Net;

namespace DesignSphere.Api.Controllers.v1.Project;


[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/project")]
public class ProjectController : BaseApiController
{
  

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> CreateProject([FromBody] CreateProjectCommandRequest query)
    {
    
        return await Mediator.Send(query);
    }

    [HttpGet("GetAllProjects")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResponseOffset<GetProjectQueryResult>), (int)HttpStatusCode.OK)]
    public async Task<PagedResponseOffset<GetProjectQueryResult>> GetAllProjects([FromQuery] GetProjectQueryRequest request)
    {
          return await Mediator.Send(request);
    }

}

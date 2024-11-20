using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Features.Project;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignSphere.Api.Controllers.v1.Project;


[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/project")]
public class ProjectController : BaseApiController
{
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResponseOffset<Ulid>), StatusCodes.Status200OK)]
    public async Task<Ulid> CreateProject([FromBody] CreateProjectCommandRequest query)
    {
    
        return await Mediator.Send(query);
    }

  

}

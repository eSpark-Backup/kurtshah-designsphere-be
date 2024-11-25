using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Features.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesignSphere.Api.Controllers.v1.Project;


[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/project")]
public class ProjectController : BaseApiController
{
    private readonly IProjectRepository _repository;

    public ProjectController(IProjectRepository repository)
    {
        _repository = repository;

    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Domain.Entites.Project>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(PagedResponseOffset<Domain.Entites.Project>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Domain.Entites.Project>>> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Domain.Entites.Project), (int)HttpStatusCode.OK)]
    public async Task<Domain.Entites.Project> CreateProject([FromBody] CreateProjectCommandRequest query)
    {
    
        return await Mediator.Send(query);
    }


}

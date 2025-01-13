using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Features.Plan;
using DesignSphere.Application.Features.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesignSphere.Api.Controllers.v1.Plan;

[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/plan")]
public class PlanController : BaseApiController
{
    [HttpGet("GetPlansByProjectId")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResponseOffset<GetProjectPlansQueryResult>), (int)HttpStatusCode.OK)]
    public async Task<PagedResponseOffset<GetProjectPlansQueryResult>> GetPlansByProjectId([FromQuery] GetProjectPlansQueryRequest request)
    {
        return await Mediator.Send(request);
    }
}
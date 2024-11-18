using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
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
    //[HttpPost("[action]")]
    //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    //[ProducesResponseType(typeof(PagedResponseOffset<ShopViewFilterV2Result>), StatusCodes.Status200OK)]
    //public async Task<PagedResponseOffset<>> ChangeView([FromBody] SearchMechanicV2Query query)
    //{
    //    return await Mediator.Send(query);
    //}
}

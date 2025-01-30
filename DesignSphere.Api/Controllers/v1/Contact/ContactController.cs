using Asp.Versioning;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Features.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesignSphere.Api.Controllers.v1.Contact;


[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/Contact")]
public class ContactController : BaseApiController
{


    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> AddContact([FromBody] AddContactRequest query)
    {

        return await Mediator.Send(query);
    }

    [HttpGet("GetProjectRoles")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<GetProjectRoleQueryResult>), (int)HttpStatusCode.OK)]
    public async Task<List<GetProjectRoleQueryResult>> GetProjectRoles()
    {
        var projectRoles = await Mediator.Send(new GetProjectRoleQueryRequest());

        return projectRoles;
    }

 
    [HttpGet("GetContacts")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResponseOffset<GetContactQueryResult>), (int)HttpStatusCode.OK)]
    public async Task<PagedResponseOffset<GetContactQueryResult>> GetContacts([FromQuery] GetContactQueryRequest request)
    {
        return await Mediator.Send(request);
    }

 
    [HttpDelete("DeleteContactById")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> DeleteContactById([FromQuery] Ulid id)
    {
        var response = await Mediator.Send(new DeleteContactRequest(id));

        return response;
    }
}

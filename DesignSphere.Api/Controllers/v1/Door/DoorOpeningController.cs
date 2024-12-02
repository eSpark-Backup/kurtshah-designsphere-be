using Asp.Versioning;
using AutoWrapper.Wrappers;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Features.Door;
using DesignSphere.Application.Features.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesignSphere.Api.Controllers.v1.Door;


[ApiController]
[Authorize(Roles = "customer1")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/door")]
public class DoorOpeningController : BaseApiController
{
    private readonly IDoorOpeningRepository _repository;

    public DoorOpeningController(IDoorOpeningRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("CreateDoorOpening")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Ulid), (int)HttpStatusCode.OK)]
    public async Task<Ulid> CreateDoorOpening([FromBody] CreateDoorOpeningCommandRequest query)
    {

        return await Mediator.Send(query);
    }


}

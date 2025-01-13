using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesignSphere.Api.Controllers.v1;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
using AutoMapper;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Contracts.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;

namespace DesignSphere.Application.Hubs;

[Authorize]
public class SecuredHub : BaseHub<ISecuredHub>
{

    private readonly ILogger<ISecuredHub> logger;
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public SecuredHub(ILogger<ISecuredHub> logger, IMediator mediator, IMapper mapper)
        : base(logger)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.mapper = mapper;
    }


    //public async Task SendMessage(string message)
    //{
    //    var transportType = Context.Features.Get<IHttpTransportFeature>().TransportType.ToString();
    //    await Clients.Client(Context.ConnectionId).SendAsync("Response", $"TransportType-{transportType} :: Data Received from SendMessage method: {message}");

    //}

    public async Task SendMessageToGroup(string message)
    {
        var groupName = GetGroupName();

        if (string.IsNullOrWhiteSpace(groupName))
        {
            logger.LogWarning("No valid group name found for the user.");
            return;
        }

        logger.LogInformation($"Sending message to group '{groupName}': {message}");

        await Clients.Group(groupName).Test($"Message from {Context.User?.Identity?.Name}: {message}");
    }

    //public async Task HandleMediatorAction()
    //{
    //    var groupName = GetGroupName();
    //    if (!string.IsNullOrWhiteSpace(groupName))
    //    {
    //        var command = new SomeMediatorCommand { GroupName = groupName };
    //        var result = await mediator.Send(command);
    //        logger.LogInformation($"Mediator handled result for group '{groupName}'");
    //    }
    //}
}

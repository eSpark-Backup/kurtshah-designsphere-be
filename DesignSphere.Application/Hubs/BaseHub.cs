using DesignSphere.Application.Contracts.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;


namespace DesignSphere.Application.Hubs;
public class BaseHub<T> : Hub<T> where T : class, IBaseHub
{
    private readonly ILogger<T> logger;

    public BaseHub(ILogger<T> logger)
    {
        this.logger = logger;
    }
    public string GetGroupName()
    {
        return Context.User?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty;

    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var groupName = GetGroupName();
        logger.LogInformation($"Hub: A client disconnected - ConnectionId:{Context.ConnectionId} - User Id:{Context.User?.Identity?.Name} - User Identifier:{Context.UserIdentifier}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).Test($"Thanks for ... {Context.User?.Identity?.Name}");
        await base.OnDisconnectedAsync(exception);
    }
    public override async Task OnConnectedAsync()
    {
        var groupName = GetGroupName();
        logger.LogInformation($"Hub: A client connected - ConnectionId:{Context.ConnectionId} - User Id:{Context.User?.Identity?.Name} - User Identifier:{Context.UserIdentifier}");
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).Test($"Thanks for ... {Context.User?.Identity?.Name}");
        await base.OnConnectedAsync();
    }
}

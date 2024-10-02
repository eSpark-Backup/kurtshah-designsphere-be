namespace EventBus.Messages.Sts;

public interface IIntegrationBaseEvent
{
    Guid EventId { get; }
}

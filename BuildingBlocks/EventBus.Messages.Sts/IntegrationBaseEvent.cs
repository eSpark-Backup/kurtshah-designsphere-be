namespace EventBus.Messages.Sts;

public class IntegrationBaseEvent : IIntegrationBaseEvent
{
    public IntegrationBaseEvent()
    {
        EventId = Guid.NewGuid();
        EventTime = DateTime.Now;
    }

    public Guid EventId { get; }
    public DateTime EventTime { get; }
    public string CreatorId { get; set; }
    public string? CreatorName { get; set; }
}

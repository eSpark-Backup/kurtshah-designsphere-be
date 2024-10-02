
namespace EventBus.Messages.Sts.Events;
public interface IUserInfoUpdate
{
    string FQDN { get; set; }
    string ClientId { get; set; }
    string Name { get; set; }
    string UserId { get; set; }
}
public class UserInfoUpdateEvent : IntegrationBaseEvent, IUserInfoUpdate
{
    public string FQDN { get; set; }
    public string ClientId { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}

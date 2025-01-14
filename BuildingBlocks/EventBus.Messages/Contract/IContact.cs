

namespace EventBus.Messages.Contract;
public interface  IContact
{
    string UserId { get; set; }
    string Name { get; set; }
    string EmailAddress { get; set; }

    Ulid ProjectRoleId { get; set; }
}

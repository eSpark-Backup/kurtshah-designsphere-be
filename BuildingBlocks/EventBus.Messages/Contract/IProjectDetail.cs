namespace EventBus.Messages.Contract;

public interface IProjectDetail
{
    Ulid ProjectId { set; get; }
    Ulid ComapanyId { get; set; }
}

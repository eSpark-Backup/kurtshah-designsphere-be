
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IDoorOpening : ISize
{
    uint Number { get; set; }
    EDoorCondition ActiveStopCondition { get; set; }
    Ulid? DoorId { get; set; }
    string? DoorDescription { get; set; }
    Ulid? DoorFrameId { get; set; }
    string? DoorFrameDescription { get; set; }
    Ulid ProjectPlanId { get; set; }
    Ulid? HardwareSetId { get; set; }
    string? HardwareSetDescription { get; set; }

}
using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Door;

public class CreateDoorOpeningCommandRequest : IRequest<Ulid>
{
    public uint Number { get; set; }
    public float Thickness { get; set; }
    public EDoorCondition ActiveStopCondition { get; set; }

    public float Width { get; set; }
    public float Height { get; set; }
    public Ulid? DoorFrameId { get; set; }
    public string? DoorFrameDescription { get; set; }
    public Ulid ProjectPlanId { get; set; }
    public Ulid? DoorId { get; set; }
    public string? DoorDescription { get; set; }
    public Ulid? HardwareSetId { get; set; }
    public string? HardwareSetDescription { get; set; }
}

public class CreateDoorOpeningCommandHandler : IRequestHandler<CreateDoorOpeningCommandRequest, Ulid>
{
    private readonly IDoorOpeningRepository _repository;

    public CreateDoorOpeningCommandHandler(IDoorOpeningRepository repository)
    {
        _repository = repository;
    }

    public async Task<Ulid> Handle(CreateDoorOpeningCommandRequest request, CancellationToken cancellationToken)
    {
        var doorOpening = new DoorOpening
        {
            Id = Ulid.NewUlid(),
            Number = request.Number,
            Thickness = request.Thickness,
            ActiveStopCondition = request.ActiveStopCondition,
            Width = request.Width,
            Height = request.Height,
            DoorFrameId = request.DoorFrameId,
            DoorFrameDescription = request.DoorFrameDescription,
            ProjectPlanId = request.ProjectPlanId,
            DoorId = request.DoorId,
            DoorDescription = request.DoorDescription,
            HardwareSetId = request.HardwareSetId,
            HardwareSetDescription = request.HardwareSetDescription
        };

        await _repository.AddAsync(doorOpening);

        return doorOpening.Id;
    }
}
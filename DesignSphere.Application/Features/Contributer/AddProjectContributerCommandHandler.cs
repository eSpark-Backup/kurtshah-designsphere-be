using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using EventBus.Messages.Contract;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Contributer;

public class AddProjectContributerRequest : IRequest<Ulid>
{
    public string Name { get; set; }
    public ContributerRole ProjectUserRole { get; set; }
    public EProjectRole ProjectRole { get; set; }
    public string UserId { get; set; }
    public Ulid ProjectId { get; set; }
}

public class AddProjectContributerCommandHandler : IRequestHandler<AddProjectContributerRequest, Ulid>

{
    private readonly IContributerRepository repository;

    public AddProjectContributerCommandHandler(IContributerRepository repository)
    {
        this.repository = repository;
    }


    public async Task<Ulid> Handle(AddProjectContributerRequest request, CancellationToken cancellationToken)
    {

        var contributer = new Domain.Entites.ProjectContributer
        {
            Name = request.Name,
            ProjectUserRole = request.ProjectUserRole,
            ProjectRole = request.ProjectRole,
            UserId = request.UserId,
            ProjectId = request.ProjectId
        };

        await repository.AddAsync(contributer);
        return contributer.Id;

    }
}

using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;

namespace DesignSphere.Application.Features.Project;

public class CreateProjectCommandRequest : IRequest<Domain.Entites.Project>
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public bool? HelpUs { get; set; }

}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommandRequest, Domain.Entites.Project>

{
    private readonly IGpsRepository repository;

    public CreateProjectCommandHandler(IGpsRepository repository)
    {
        this.repository = repository;
    }


    public async Task<Domain.Entites.Project> Handle(CreateProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = new Domain.Entites.Project
        {
            Id = Ulid.NewUlid(),
            Name = request.Name,
            PublishDate = request.PublishDate,
            DeviverableDate = request.DeviverableDate,
            Stage = request.Stage,
            Type = request.Type,
            HelpUs = request.HelpUs
        };

        await repository.AddAsync(project);

        return project;
    }
}

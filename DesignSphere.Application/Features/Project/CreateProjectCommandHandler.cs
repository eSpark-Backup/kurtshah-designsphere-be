using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using SieveCustom.Models;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Project;

public class CreateProjectCommandRequest : IRequest<Ulid>
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public bool? HelpUs { get; set; }
    public string? Comment { get; set; }


}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommandRequest, Ulid>

{
    private readonly IProjectRepository repository;

    public CreateProjectCommandHandler(IProjectRepository repository)
    {
        this.repository = repository;
    }


    public async Task<Ulid> Handle(CreateProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = new Domain.Entites.Project
        {
            Id = Ulid.NewUlid(),
            Name = request.Name,
            PublishDate = request.PublishDate,
            DeviverableDate = request.DeviverableDate,
            Stage = request.Stage,
            Type = request.Type,
            HelpUs = request.HelpUs,
            Comment = request.Comment
        };

        await repository.AddAsync(project);


        return project.Id;
        
    }
}

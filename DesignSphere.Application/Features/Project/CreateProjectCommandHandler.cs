using DesignSphere.Application.Common;
using DesignSphere.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DesignSphere.Application.Features.Project;

public class CreateProjectCommandRequest : IRequest<Ulid>
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public bool? HelpUs { get; set; }

}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommandRequest, Ulid>

{
    private readonly IDbGenericRepository<Domain.Entites.Project> _repository;


    public CreateProjectCommandHandler(IDbGenericRepository<Domain.Entites.Project> repository)
    {
        _repository = repository;
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
            HelpUs = request.HelpUs
        };

        await _repository.AddAsync(project, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}

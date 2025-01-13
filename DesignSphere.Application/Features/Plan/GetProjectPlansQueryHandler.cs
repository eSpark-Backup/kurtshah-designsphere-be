using AutoMapper;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using EventBus.Messages.Contract;
using SieveCustom.Models;


namespace DesignSphere.Application.Features.Plan;


public class GetProjectPlansQueryResult : IProjectPlan
{
    public Ulid ProjectId { get; set; }
    public string Name { get ; set ; }
    public string Description { get ; set ; }
}

public class GetProjectPlansQueryRequest : SieveModel, IRequest<PagedResponseOffset<GetProjectPlansQueryResult>>
{
    public Ulid ProjectId { get; set; }
}

public class GetProjectListQueryHandler : IRequestHandler<GetProjectPlansQueryRequest, PagedResponseOffset<GetProjectPlansQueryResult>>
{
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;

    public GetProjectListQueryHandler(IPlanRepository repository, IMapper mapper
)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponseOffset<GetProjectPlansQueryResult>> Handle(GetProjectPlansQueryRequest request, CancellationToken cancellationToken)
    {
        var ProjectIdFilter = $"ProjectId=={request.ProjectId}";

        var sieveModel = new SieveModel
        {
            Filters = string.IsNullOrEmpty(request.Filters) ? ProjectIdFilter : $"{request.Filters},{ProjectIdFilter}",
            Sorts = "-CreationTime",
            Page = request.Page,
            PageSize = request.PageSize
        };

        var projects = await _repository.GetFilteredAsync(sieveModel);


        var result = new PagedResponseOffset<GetProjectPlansQueryResult>(
    _mapper.Map<List<GetProjectPlansQueryResult>>(projects.Data),
    projects.PageNumber,
    projects.PageSize,
    projects.TotalRecords
);

        return result;
    }
}
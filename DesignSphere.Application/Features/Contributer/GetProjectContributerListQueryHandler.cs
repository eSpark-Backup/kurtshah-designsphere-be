using AutoMapper;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Common;
using EventBus.Messages.Contract;
using SieveCustom.Models;

namespace DesignSphere.Application.Features.Contributer;


public class GetContributerQueryResult : EntityBase, IProjectContributer
{

   public Ulid ContactId { get ; set ; }
    public string ContactName { get; set; }

    public Ulid ProjectId { get; set; }
    public Ulid ProjectRoleId { get; set; }
    public string ProjectRoleName { get; set; }

    public bool IsContributerRemoved { get; set ; }

}

public class GetContributerQueryRequest : SieveModel, IRequest<PagedResponseOffset<GetContributerQueryResult>>
{
    public Ulid ProjectId { get; set; }
}

public class GetProjectContributerListQueryHandler : IRequestHandler<GetContributerQueryRequest, PagedResponseOffset<GetContributerQueryResult>>
{
    private readonly IContributerRepository _repository;
    private readonly IMapper _mapper;

    public GetProjectContributerListQueryHandler(IContributerRepository repository, IMapper mapper
)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponseOffset<GetContributerQueryResult>> Handle(GetContributerQueryRequest request, CancellationToken cancellationToken)
    {
        var projectFilter = $"ProjectId=={request.ProjectId}";

        var sieveModel = new SieveModel
        {
            Filters = string.IsNullOrEmpty(request.Filters) ? projectFilter : $"{request.Filters},{projectFilter}",
            Sorts = "-CreationTime",
            Page = request.Page,
            PageSize = request.PageSize
        };

        var projects = await _repository.GetFilteredAsync(sieveModel);

        var result = new PagedResponseOffset<GetContributerQueryResult>(
    _mapper.Map<List<GetContributerQueryResult>>(projects.Data),
    projects.PageNumber,
    projects.PageSize,
    projects.TotalRecords
);

        return result;
    }
}
using AutoMapper;
using BuildingBlocks.Common.Models;
using BuildingBlocks.Common.Services;
using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Common;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SieveCustom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Project;

public class GetProjectQueryResult : EntityBase, IProject
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public string StageName { get; set; }

    public EProjectType Type { get; set; }
    public string TypeName { get; set; }

    public HelpOption HelpUs { get; set; }
    public string HelpUsName { get; set; }

    public string? OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public string? CompanyName { get; set; }
   // public long? ProjectPlans { get; set; }
    public long? DoorOpenings { get; set; }

    public bool Revit { get; set; } = true;

}

public class GetProjectQueryRequest : SieveModel, IRequest<PagedResponseOffset<GetProjectQueryResult>>
{
}

public class GetProjectListQueryHandler : IRequestHandler<GetProjectQueryRequest, PagedResponseOffset<GetProjectQueryResult>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public GetProjectListQueryHandler(IProjectRepository repository, IMapper mapper, IUserService userService
)
    {
        _repository = repository;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<PagedResponseOffset<GetProjectQueryResult>> Handle(GetProjectQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdentity();
        var ownerFilter = $"OwnerId=={userId}";

        var sieveModel = new SieveModel
        {
            Filters = string.IsNullOrEmpty(request.Filters) ? ownerFilter : $"{request.Filters},{ownerFilter}",
            Sorts = "-CreationTime",
            Page = request.Page,
            PageSize = request.PageSize
        };

        var projects = await _repository.GetFilteredAsync(sieveModel, includeProperties: "ProjectPlans,ProjectPlans.DoorOpenings");


        var result = new PagedResponseOffset<GetProjectQueryResult>(
    _mapper.Map<List<GetProjectQueryResult>>(projects.Data),
    projects.PageNumber,
    projects.PageSize,
    projects.TotalRecords
);

        return result;
    }
}
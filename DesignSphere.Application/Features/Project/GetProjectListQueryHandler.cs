using AutoMapper;
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
    public string? CompanyName { get; set ; }
}

public class GetProjectQueryRequest : SieveModel, IRequest<List<GetProjectQueryResult>>
{


}


public class GetProjectListQueryHandler : IRequestHandler<GetProjectQueryRequest, List<GetProjectQueryResult>>
{
    private readonly IProjectRepository repository;
    private readonly IMapper _mapper;

    public GetProjectListQueryHandler(IProjectRepository repository, IMapper mapper
)
    {
        this.repository = repository;
        _mapper = mapper;
    }

    public async Task<List<GetProjectQueryResult>> Handle(GetProjectQueryRequest request,CancellationToken cancellationToken)
    {
        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = string.IsNullOrEmpty(request.Sorts) ? "CreationTime" : request.Sorts, // Default to CreatedOn if Sorts is empty
            Page = request.Page,       
            PageSize = request.PageSize 
        };

        var projects = await repository.GetProjectsAsync(sieveModel);

        var result = _mapper.Map<List<GetProjectQueryResult>>(projects);

        return result;
  
    }
}

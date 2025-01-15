using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Contributer;

public class AddProjectContributerRequest : IRequest<ApiResponse>
{
    public Ulid ContactId { get; set; }
    public Ulid ProjectRoleId { get; set; }
    public Ulid ProjectId { get; set; }
}

public class AddProjectContributerCommandHandler : IRequestHandler<AddProjectContributerRequest, ApiResponse>

{
    private readonly IContributerRepository repository;

    public AddProjectContributerCommandHandler(IContributerRepository repository)
    {
        this.repository = repository;
    }


    public async Task<ApiResponse> Handle(AddProjectContributerRequest request, CancellationToken cancellationToken)
    {

        bool isContributerExist = await repository.getContributerByProject(request.ContactId, request.ProjectId);

        if(isContributerExist)
        {
            return new ApiResponse(409,"Contributer Already Exist");
        }

        var contributer = new Domain.Entites.ProjectContributer
        {
    
            ContactId = request.ContactId,
            ProjectRoleId = request.ProjectRoleId,
            ProjectId = request.ProjectId
        };

        await repository.AddAsync(contributer);

        return new ApiResponse(200, "Contributer Successfully Added ");
    }
}

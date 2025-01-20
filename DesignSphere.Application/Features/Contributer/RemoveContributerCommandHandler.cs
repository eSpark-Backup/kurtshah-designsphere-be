using AutoMapper;
using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using SieveCustom.Models;


namespace DesignSphere.Application.Features.Contributer;

public class RemoveProjectContributerRequest : IRequest<ApiResponse>
{
    public Ulid ContributerId { get; set; }
}

public class RemoveContributerCommandHandler : IRequestHandler<RemoveProjectContributerRequest,ApiResponse>
{
    private readonly IContributerRepository _repository;
    private readonly IProjectRoleRepository _projectRoleRepository;


    public RemoveContributerCommandHandler(IContributerRepository repository, IProjectRoleRepository projectRoleRepository
)
    {
        _repository = repository;
        _projectRoleRepository = projectRoleRepository;
    }

    public async Task<ApiResponse> Handle(RemoveProjectContributerRequest request, CancellationToken cancellationToken)
    {
     

        var contributer = await _repository.GetByIdAsync(request.ContributerId);

        var getProjectRole = await _projectRoleRepository.getProjectRole("Owner");

        if (contributer.ProjectRoleId == getProjectRole.Id)
        {
            return new ApiResponse("No Delete Permissions Allowed for Project Owner");
        }

        contributer.IsContributerRemoved = true;
        await _repository.UpdateAsync(contributer);

        return new ApiResponse("Contributer removed successfully");
    }
} 
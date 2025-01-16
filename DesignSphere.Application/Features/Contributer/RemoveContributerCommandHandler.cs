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

    public RemoveContributerCommandHandler(IContributerRepository repository
)
    {
        _repository = repository;
    }

    public async Task<ApiResponse> Handle(RemoveProjectContributerRequest request, CancellationToken cancellationToken)
    {
        var contributer = await _repository.GetByIdAsync(request.ContributerId);

        contributer.IsContributerRemoved = true;
        await _repository.UpdateAsync(contributer);

        return new ApiResponse("Contributer removed successfully");
    }
} 
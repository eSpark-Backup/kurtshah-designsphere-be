using AutoWrapper.Wrappers;
using DesignSphere.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Application.Features.Contributer;

public class UpdateProjectContributerRequest : IRequest<ApiResponse>
{
    public Ulid ProjectContributerId { get; set; }
    public Ulid ProjectRoleId { get; set; }
}

public class UpdateProjectContributerCommandHandler : IRequestHandler<UpdateProjectContributerRequest, ApiResponse>

{
    private readonly IContributerRepository repository;


    public UpdateProjectContributerCommandHandler(IContributerRepository repository)
    {
        this.repository = repository;
    }


    public async Task<ApiResponse> Handle(UpdateProjectContributerRequest request, CancellationToken cancellationToken)
    {

        var conributer = await repository.GetByIdAsync(request.ProjectContributerId);

        conributer.ProjectRoleId = request.ProjectRoleId;

        await repository.UpdateAsync(conributer);

        return new ApiResponse(200, "Contributer Successfully Updated ");
    }
}

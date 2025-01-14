
using AutoMapper;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Common;
using EventBus.Messages.Contract;


namespace DesignSphere.Application.Features.Contact;


public class GetProjectRoleQueryResult : EntityBase, IProjectRole
{

    public Ulid ProjectRoleId { get; set; }

    public string Name { get; set; }
}

public class GetProjectRoleQueryRequest : IRequest<List<GetProjectRoleQueryResult>>
{
}

public class GetProjectRolesQueryHandler :IRequestHandler<GetProjectRoleQueryRequest, List<GetProjectRoleQueryResult>>
{
    private readonly IProjectRoleRepository _repository;
    private readonly IMapper _mapper;

    public GetProjectRolesQueryHandler(IProjectRoleRepository repository, IMapper mapper
)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<GetProjectRoleQueryResult>> Handle(GetProjectRoleQueryRequest request,CancellationToken cancellationToken)
    {
        
        var projectRoles = await _repository.GetAllAsync();

        var result = _mapper.Map<List<GetProjectRoleQueryResult>>(projectRoles);

        return result;
    }
}
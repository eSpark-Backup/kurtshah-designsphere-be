using DesignSphere.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Application.Features.Contributer;

public class IsContributerExistRequest : IRequest<bool>
{
    public Ulid ProjectId { get; set; }
    public Ulid ContactId { get; set; }
}

public class ExistContributerQueryHandler : IRequestHandler<IsContributerExistRequest, bool>
{
    private readonly IContributerRepository _contributerRepository;

    public ExistContributerQueryHandler(IContributerRepository contributerRepository)
    {
        _contributerRepository = contributerRepository;
    }

    public async Task<bool> Handle(IsContributerExistRequest request, CancellationToken cancellationToken)
    {
        return await _contributerRepository.getContributerByProject(request.ContactId, request.ProjectId);
    }
}
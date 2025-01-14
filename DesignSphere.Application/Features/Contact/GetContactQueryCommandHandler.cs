using AutoMapper;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Features.Project;
using DesignSphere.Domain.Common;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using SieveCustom.Models;

namespace DesignSphere.Application.Features.Contact;

public class GetContactQueryResult : EntityBase, IContact
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }

    public Ulid ProjectRoleId { get; set; }
}

public class GetContactQueryRequest : SieveModel, IRequest<PagedResponseOffset<GetContactQueryResult>>
{
    public string? Name { get; set; }
}

public class GetContactQueryCommandHandler : IRequestHandler<GetContactQueryRequest, PagedResponseOffset<GetContactQueryResult>>

{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public GetContactQueryCommandHandler(IContactRepository repository, IMapper mapper
)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponseOffset<GetContactQueryResult>> Handle(GetContactQueryRequest request, CancellationToken cancellationToken)
    {
        var nameFilter = $"Name=={request.Name}";

        var sieveModel = new SieveModel
        {
            Filters = string.IsNullOrEmpty(request.Name)
                ? request.Filters
                : $"{request.Filters},Name=={request.Name}",

            Sorts = "-CreationTime",
            Page = request.Page,
            PageSize = request.PageSize
        };

        var contacts = await _repository.GetFilteredAsync(sieveModel);

        var result = new PagedResponseOffset<GetContactQueryResult>(
   _mapper.Map<List<GetContactQueryResult>>(contacts.Data),
   contacts.PageNumber,
   contacts.PageSize,
   contacts.TotalRecords
);
        return result;
    }
}
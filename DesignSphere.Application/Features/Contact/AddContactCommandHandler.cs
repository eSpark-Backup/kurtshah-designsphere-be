using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Contact;


public class AddContactRequest : IRequest<Ulid>
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }

}

public class AddContactCommandHandler : IRequestHandler<AddContactRequest, Ulid>

{
    private readonly IContactRepository repository;

    public AddContactCommandHandler(IContactRepository repository)
    {
        this.repository = repository;
    }


    public async Task<Ulid> Handle(AddContactRequest request, CancellationToken cancellationToken)
    {

        var contact = new Domain.Entites.Contact
        {
            Name = request.Name,
            EmailAddress = request.EmailAddress,
            UserId = request.UserId,
        };

        await repository.AddAsync(contact);
        return contact.Id;

    }
}

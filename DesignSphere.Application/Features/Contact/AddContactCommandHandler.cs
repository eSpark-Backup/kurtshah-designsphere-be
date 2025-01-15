using AutoWrapper.Wrappers;
using DesignSphere.Application.Contracts;


namespace DesignSphere.Application.Features.Contact;


public class AddContactRequest : IRequest<ApiResponse>
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }

}

public class AddContactCommandHandler : IRequestHandler<AddContactRequest, ApiResponse>

{
    private readonly IContactRepository repository;

    public AddContactCommandHandler(IContactRepository repository)
    {
        this.repository = repository;
    }


    public async Task<ApiResponse> Handle(AddContactRequest request, CancellationToken cancellationToken)
    {
        // Check if a contact with the given UserId already exists
        var existingContact = await repository.getContactByUserId(request.UserId);

        if (existingContact != null)
        {
            return new ApiResponse(404, "Contact Already Exist ");

        }

        var contact = new Domain.Entites.Contact
        {
            Name = request.Name,
            EmailAddress = request.EmailAddress,
            UserId = request.UserId,
        };

        await repository.AddAsync(contact);

        return new ApiResponse(200, "Contact Successfully Added ");

    }

}

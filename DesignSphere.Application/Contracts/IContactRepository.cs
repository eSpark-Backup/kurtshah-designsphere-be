using DesignSphere.Domain.Entites;


namespace DesignSphere.Application.Contracts;

public interface IContactRepository : IAsyncRepository<Contact>
{
    Task<Contact> getContactByUserId(string userId);

}

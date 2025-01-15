using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SieveCustom.Services;


namespace DesignSphere.Infrastructure.Repositries;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    private readonly ILogger<ContactRepository> _logger;

    public ContactRepository(SQLContext dbContext, ILogger<ContactRepository> logger, ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
        _logger = logger;
    }

    public async Task<Contact> getContactByUserId(string userId)
    {
        return await dbContext.Contacts
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

}

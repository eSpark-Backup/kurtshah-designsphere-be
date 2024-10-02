using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository;

public class ExExUserIdentityRepository : IExExUserIdentityRepository
{
    private readonly ILogger<ExExUserIdentityRepository> logger;
    private readonly MylocalContext dbContext;

    public ExExUserIdentityRepository(MylocalContext dbContext) 
    {
        this.dbContext = dbContext;
    }

    public async Task<string> AddUser(ExUserIdentity user)
    {
        var result =  dbContext.ExUserIdentities.Add(user);
        dbContext.SaveChanges();
        return result.Entity.Id;
    }

    public void DeleteUser(ExUserIdentity user)
    {
        dbContext.ExUserIdentities.Remove(user);
    }
}


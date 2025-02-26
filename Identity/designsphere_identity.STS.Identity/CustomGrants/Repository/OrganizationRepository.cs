using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository;

public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
{
    private readonly ILogger<UserDetailRepository> _logger;

    public OrganizationRepository(MylocalContext dbContext, ILogger<UserDetailRepository> logger) 
        : base(dbContext)
    {
        _logger = logger;
    }

    public async Task<IReadOnlyList<Organization>> GetOrganizationsAsync()
    {
        return await GetAsync(x=> x.IsActive && x.IsDeleted);
    }
    public async Task<Organization> GetOrganizationAsync(Guid orgId)
    {
        return await GetByIdAsync(orgId, x => x.IsActive && x.IsDeleted);
    }

    public async Task<Organization> PostOrganizationAsync(Organization request)
    {
        return await AddAsync(request);
    }

    public async Task<bool> PutOrganizationAsync(Organization request)
    {
        return await UpdateAsync(request);
    }

    public async Task<bool> DeleteOrganizationAsync(Guid orgId)
    {
        return await SoftDeleteByIdAsync(orgId);
    }
}

using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;

public interface IOrganizationRepository : IAsyncRepository<Organization>
{
    Task<Organization> GetOrganizationAsync(Guid orgId);
    Task<IReadOnlyList<Organization>> GetOrganizationsAsync();
    Task<Organization> PostOrganizationAsync(Organization request);
    Task<bool> PutOrganizationAsync(Organization request);
    Task<bool> DeleteOrganizationAsync(Guid orgId);
}

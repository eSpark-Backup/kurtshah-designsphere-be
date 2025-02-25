using designsphere_identity.Admin.EntityFramework.Shared.DbContexts;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace designsphere_identity.STS.Identity.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MylocalContext _globalDbContext;

    public TenantMiddleware(RequestDelegate next, MylocalContext globalDbContext)
    {
        _next = next;
        _globalDbContext = globalDbContext;
    }

    public async Task Invoke(HttpContext context)
    {
        var tenantIdentifier = context.Request.Headers["X-Tenant"].FirstOrDefault()
                               ?? context.Request.Query["tenant"].FirstOrDefault();

        if (!string.IsNullOrEmpty(tenantIdentifier))
        {
            var organization = await _globalDbContext.Organizations
                .FirstOrDefaultAsync(o => o.Domain == tenantIdentifier || o.Id == tenantIdentifier);

            if (organization != null)
            {
                context.Items["Tenant"] = organization.Id;
                context.Items["ConnectionString"] = organization.ConnectionString;
            }
        }

        await _next(context);
    }
}

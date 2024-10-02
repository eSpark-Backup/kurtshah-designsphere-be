using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using designsphere_identity.Admin.EntityFramework.Shared.DbContexts;
using designsphere_identity.STS.Identity.Helpers;

namespace designsphere_identity.STS.Identity.Configuration.Test;

public class StartupTest : Startup
{
    public StartupTest(IWebHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
    {
    }

    public override void RegisterDbContexts(IServiceCollection services)
    {
        services.RegisterDbContextsStaging<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, IdentityServerDataProtectionDbContext>();
    }
}

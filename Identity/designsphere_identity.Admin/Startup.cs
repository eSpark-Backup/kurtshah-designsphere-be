using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Shared.Configuration.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using designsphere_identity.Admin.EntityFramework.Shared.DbContexts;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.Admin.Configuration.Database;
using designsphere_identity.Admin.Helpers;
using designsphere_identity.Shared.Dtos;
using designsphere_identity.Shared.Dtos.Identity;

namespace designsphere_identity.Admin;

public class Startup
{
    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        HostingEnvironment = env;
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment HostingEnvironment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        // Adds the IdentityServer4 Admin UI with custom options.
        services.AddIdentityServer4AdminUI<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext,
        AdminLogDbContext, AdminAuditLogDbContext, AuditLog, IdentityServerDataProtectionDbContext,
            UserIdentity, UserIdentityRole, UserIdentityUserClaim, UserIdentityUserRole,
            UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken, string,
            IdentityUserDto, IdentityRoleDto, IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
            IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
            IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto>(ConfigureUIOptions);

        // Monitor changes in Admin UI views
        services.AddAdminUIRazorRuntimeCompilation(HostingEnvironment);

        // Add email senders which is currently setup for SendGrid and SMTP
        services.AddEmailSenders(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseSerilogRequestLogging(options =>
        {
            // Customize the message template
            //options.MessageTemplate = "Handled {RequestPath}";

            // Emit debug-level events instead of the defaults
            //options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

            // Attach additional properties to the request completion event
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                //diagnosticContext.Set(Constants.IdentityClaims.UserId, httpContext..Connection.cha userContext.UserId);
                //diagnosticContext.Set(Constants.HeaderTypes.Channel, userContext.Channel);
                //diagnosticContext.Set(Constants.HeaderTypes.UserAgent, requestContext.UserAgent);
                //diagnosticContext.Set(Constants.HeaderTypes.CorrelationId, requestContext.CorrelationId);
                diagnosticContext.Set(propertyName: "Requester IP", httpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
                //diagnosticContext.Set("RequestBody", httpContext.Request.Body);
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserId", httpContext.User.Identity.Name);
            };
        });
        app.UseRouting();

        app.UseIdentityServer4AdminUI();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapIdentityServer4AdminUI();
            endpoint.MapIdentityServer4AdminUIHealthChecks();
        });
    }

    public virtual void ConfigureUIOptions(IdentityServer4AdminUIOptions options)
    {
        // Applies configuration from appsettings.
        options.BindConfiguration(Configuration);
        if (HostingEnvironment.IsDevelopment())
        {
            options.Security.UseDeveloperExceptionPage = true;
        }
        else
        {
            options.Security.UseHsts = true;
        }

        // Set migration assembly for application of db migrations
        var migrationsAssembly = MigrationAssemblyConfiguration.GetMigrationAssemblyByProvider(options.DatabaseProvider);
        options.DatabaseMigrations.SetMigrationsAssemblies(migrationsAssembly);

        // Use production DbContexts and auth services.
        options.Testing.IsStaging = false;
    }
}

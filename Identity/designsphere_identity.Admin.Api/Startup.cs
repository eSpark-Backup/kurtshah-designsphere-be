using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Shared.Configuration.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using designsphere_identity.Admin.Api.Configuration;
using designsphere_identity.Admin.Api.Configuration.Authorization;
using designsphere_identity.Admin.Api.ExceptionHandling;
using designsphere_identity.Admin.Api.Mappers;
using designsphere_identity.Admin.Api.Resources;
using designsphere_identity.Admin.EntityFramework.Shared.DbContexts;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.Shared.Dtos;
using designsphere_identity.Shared.Dtos.Identity;
using designsphere_identity.Admin.Api.Helpers;

namespace designsphere_identity.Admin.Api;

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
        var adminApiConfiguration = Configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();
        services.AddSingleton(adminApiConfiguration);

        // Add DbContexts
        RegisterDbContexts(services);

        services.AddDataProtection<IdentityServerDataProtectionDbContext>(Configuration);

        // Add email senders which is currently setup for SendGrid and SMTP
        services.AddEmailSenders(Configuration);

        services.AddScoped<ControllerExceptionFilterAttribute>();
        services.AddScoped<IApiErrorResources, ApiErrorResources>();

        // Add authentication services
        RegisterAuthentication(services);

        // Add authorization services
        RegisterAuthorization(services);

        var profileTypes = new HashSet<Type>
        {
            typeof(IdentityMapperProfile<IdentityRoleDto, IdentityUserRolesDto, string, IdentityUserClaimsDto, IdentityUserClaimDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto, IdentityRoleClaimDto, IdentityRoleClaimsDto>)
        };

        services.AddAdminAspNetIdentityServices<AdminIdentityDbContext, IdentityServerPersistedGrantDbContext,
            IdentityUserDto, IdentityRoleDto, UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole,
            UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
            IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
            IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
            IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto>(profileTypes);

        services.AddAdminServices<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext>();

        services.AddAdminApiCors(adminApiConfiguration);

        services.AddMvcServices<IdentityUserDto, IdentityRoleDto,
            UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole,
            UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
            IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
            IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
            IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(adminApiConfiguration.ApiVersion, new OpenApiInfo { Title = adminApiConfiguration.ApiName, Version = adminApiConfiguration.ApiVersion });

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                        TokenUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/token"),
                        Scopes = new Dictionary<string, string> {
                            { adminApiConfiguration.OidcApiName, adminApiConfiguration.ApiName }
                        }
                    }
                }
            });
            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        services.AddAuditEventLogging<AdminAuditLogDbContext, AuditLog>(Configuration);

        services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, AdminLogDbContext, AdminAuditLogDbContext, IdentityServerDataProtectionDbContext>(Configuration, adminApiConfiguration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AdminApiConfiguration adminApiConfiguration)
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
        app.AddForwardHeaders();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"{adminApiConfiguration.ApiBaseUrl}/swagger/v1/swagger.json", adminApiConfiguration.ApiName);

            c.OAuthClientId(adminApiConfiguration.OidcSwaggerUIClientId);
            c.OAuthAppName(adminApiConfiguration.ApiName);
            c.OAuthUsePkce();
        });

        app.UseRouting();
        UseAuthentication(app);
        app.UseCors();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }

    public virtual void RegisterDbContexts(IServiceCollection services)
    {
        services.AddDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, AdminAuditLogDbContext, IdentityServerDataProtectionDbContext, AuditLog>(Configuration);
    }

    public virtual void RegisterAuthentication(IServiceCollection services)
    {
        services.AddApiAuthentication<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(Configuration);
    }

    public virtual void RegisterAuthorization(IServiceCollection services)
    {
        services.AddAuthorizationPolicies();
    }

    public virtual void UseAuthentication(IApplicationBuilder app)
    {
        app.UseAuthentication();
    }
}

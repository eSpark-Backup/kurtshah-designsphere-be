using EventBus.Messages.Sts;
using EventBus.Messages.Sts.Events;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Skoruba.IdentityServer4.Shared.Configuration.Helpers;
using System;
using designsphere_identity.Admin.EntityFramework.Shared.DbContexts;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.Configuration;
using designsphere_identity.STS.Identity.Configuration.Constants;
using designsphere_identity.STS.Identity.Configuration.Interfaces;
using designsphere_identity.STS.Identity.CustomGrants;
using designsphere_identity.STS.Identity.CustomGrants.Configurations;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using designsphere_identity.STS.Identity.CustomGrants.Repository;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Services;
using designsphere_identity.STS.Identity.Helpers;

namespace designsphere_identity.STS.Identity;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var rootConfiguration = CreateRootConfiguration();
        services.AddSingleton(rootConfiguration);
        // Register DbContexts for IdentityServer and Identity
        RegisterDbContexts(services);
        // Save data protection keys to db, using a common application name shared between Admin and STS
        services.AddDataProtection<IdentityServerDataProtectionDbContext>(Configuration);

        // Add email senders which is currently setup for SendGrid and SMTP
        services.AddEmailSenders(Configuration);

        // Add services for authentication, including Identity model and external providers
        RegisterAuthentication(services);

        // Add HSTS options
        RegisterHstsOptions(services);

        // Add all dependencies for Asp.Net Core Identity in MVC - these dependencies are injected into generic Controllers
        // Including settings for MVC and Localization
        // If you want to change primary keys or use another db model for Asp.Net Core Identity:
        services.AddMvcWithLocalization<UserIdentity, string>(Configuration);

        // Add authorization policies for MVC
        RegisterAuthorization(services);

        services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, IdentityServerDataProtectionDbContext>(Configuration);

        // Add RabitMQ Configuration 
        // MassTransit-RabbitMQ Configuration
        // MassTransit-RabbitMQ Configuration
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(2)));
                cfg.UseMessageRetry(r => r.Immediate(5));
                cfg.PrefetchCount = 1;
                cfg.UseConcurrencyLimit(1);

                cfg.Host(Configuration["EventBusSettings:HostAddress"]);

                cfg.Durable = bool.Parse(Configuration["EventBusSettings:IsDurable"]);
                var autoDelete = bool.Parse(Configuration["EventBusSettings:AutoDelete"]);
                cfg.AutoDelete = autoDelete;

                cfg.Message<UserInfoUpdateEvent>(e => { e.SetEntityName(EventBusConstants.UserInfoUpdateExchange); });
                cfg.Publish<UserInfoUpdateEvent>(e => { e.BindQueue(EventBusConstants.UserInfoUpdateExchange, EventBusConstants.UserInfoUpdateQueue); });

            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

        app.UseCookiePolicy();
        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            Console.WriteLine($"Migrating MylocalContext");
            var context = serviceScope.ServiceProvider.GetService<MylocalContext>();
            context.Database.Migrate();
        }
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UsePathBase(Configuration.GetValue<string>("BasePath"));

        app.UseStaticFiles();
        UseAuthentication(app);

        // Add custom security headers
        app.UseSecurityHeaders(Configuration);

        app.UseMvcLocalizationServices();

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapDefaultControllerRoute();
            endpoint.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }

    public virtual void RegisterDbContexts(IServiceCollection services)
    {
        services.RegisterDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, IdentityServerDataProtectionDbContext>(Configuration);
    }

    public virtual void RegisterAuthentication(IServiceCollection services)
    {
        services.AddAuthenticationServices<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(Configuration);
        services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity>(Configuration)
            .AddExtensionGrantValidator<PhoneGrantValidator>()
            .AddExtensionGrantValidator<GoogleGrantValidator>()
            .AddExtensionGrantValidator<AppleGrantValidator>()
            .AddExtensionGrantValidator<FacebookGrantValidator>()
            .AddExtensionGrantValidator<BackendGrantValidator>()
            .AddExtensionGrantValidator<EmailGrantValidator>()
                .AddProfileService<IdentityProfileService<UserIdentity>>()//need to be implement if required
                ;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<MylocalContext>(options => options.UseNpgsql(Configuration.GetConnectionString("IdentityDbConnection")));
        services.AddScoped<IUserDetailRepository, UserDetailRepository>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped(typeof(IUserDetailRepository), typeof(UserDetailRepository));
        services.AddScoped(typeof(IExExUserIdentityRepository), typeof(ExExUserIdentityRepository));
        services.AddScoped(typeof(IDeviceRepository), typeof(DeviceRepository));

        services.Configure<OTPConfig>(Configuration.GetSection("OTPConfig"));
        services.AddSingleton<IOTPConfig>(serviceProvider => serviceProvider.GetRequiredService<IOptions<OTPConfig>>().Value);
        services.AddScoped<IOTPService, OTPService>();
        services.AddScoped<IUserService, UserService>();

        services.Configure<SMSConfig>(Configuration.GetSection("SMSConfig"));
        services.AddSingleton<ISMSConfig>(serviceProvider => serviceProvider.GetRequiredService<IOptions<SMSConfig>>().Value);
        services.AddTransient<ISmsService, SmsService>();

        services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));
        services.AddSingleton<IEmailConfig>(serviceProvider => serviceProvider.GetRequiredService<IOptions<EmailConfig>>().Value);
        services.AddScoped<IEmailService, EmailService>();

    }

    public virtual void RegisterAuthorization(IServiceCollection services)
    {
        var rootConfiguration = CreateRootConfiguration();
        services.AddAuthorizationPolicies(rootConfiguration);
    }

    public virtual void UseAuthentication(IApplicationBuilder app)
    {
        IdentityModelEventSource.ShowPII = true;
        app.UseIdentityServer();
    }

    public virtual void RegisterHstsOptions(IServiceCollection services)
    {
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });
    }

    protected IRootConfiguration CreateRootConfiguration()
    {
        var rootConfiguration = new RootConfiguration();
        Configuration.GetSection(ConfigurationConsts.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
        Configuration.GetSection(ConfigurationConsts.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);
        return rootConfiguration;
    }
}

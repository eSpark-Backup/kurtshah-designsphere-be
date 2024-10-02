using Asp.Versioning;
using BuildingBlocks.Common.Infrastructure;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Common.Extensions;
public static class ServicesRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                //options.SupportedTokens = SupportedTokens.Reference;
                options.TokenRetriever = request => TokenRetrieval.FromAuthorizationHeader()(request);
                options.Authority = configuration.GetValue<string>("AdminConfiguration:IdentityServerBaseUrl");
                options.TokenRetriever = CustomTokenRetriever.FromHeaderAndQueryString;

                // Reference Tokens - will contact the introspection endpoint found in the discovery document to validate the token
                options.ApiName = configuration.GetValue<string>("AdminConfiguration:ApiName");
                options.ApiSecret = configuration.GetValue<string>("AdminConfiguration:ApiSecret");
            });
        return services;

    }
    public static IServiceCollection AddApiVersioningServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
        })
            .AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        return services;
    }
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()} });
        });
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        return services;
    }

    public static IServiceCollection AddApiCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var corsAllowAnyOrigin = configuration.GetValue<bool>("AdminConfiguration:CorsAllowAnyOrigin");
            options.AddDefaultPolicy(
                builder =>
                {
                    if (corsAllowAnyOrigin)
                    {
                        builder.AllowAnyOrigin();
                    }
                    else
                    {
                        var section = configuration.GetSection("AdminConfiguration:CorsAllowOrigins");
                        var cors = section.Get<string[]>();
                        builder.WithOrigins(cors);
                    }

                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
        });

        return services;
    }
}

using Microsoft.Extensions.Configuration;
using DesignSphere.Infrastructure;
using DesignSphere.Application;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Reflection;
using BuildingBlocks.Common;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices(configuration);

builder.Services.AddIdentityCore<AppUser>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options =>
{
    options.BearerTokenExpiration = TimeSpan.FromSeconds(configuration.GetValue<int>("BearerToken:BearerTokenExpiration"));
    options.RefreshTokenExpiration = TimeSpan.FromSeconds(configuration.GetValue<int>("BearerToken:RefreshTokenExpiration"));
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MigrateDatabase<SQLContext>((context, services) =>
{
    var logger = services.GetService<ILogger<SqlDbContextSeed>>();
    SqlDbContextSeed.SeedAsync(context, logger).Wait();
}); 
app.MigrateDatabase<AppDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<AppDbContextSeed>>();
    AppDbContextSeed.SeedAsync(context, logger).Wait();
}); 

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<AppUser>();
app.MapControllers();

app.Run();

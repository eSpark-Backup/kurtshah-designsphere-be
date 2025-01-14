using DesignSphere.Infrastructure;
using DesignSphere.Application;
using DesignSphere.Infrastructure.Persistence;
using BuildingBlocks.Common.Extensions;
using BuildingBlocks.Common.Infrastructure;
using Serilog;
using BuildingBlocks.Common.Services;
using Microsoft.AspNetCore.Authentication;
using BuildingBlocks.Common.Filters;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
builder.Services.AddControllers();

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddApiCors(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddApiVersioningServices(configuration);

builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices(configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));

bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing";
if (!isDevelopment)
    builder.Services.AddIdentityServices(configuration);
else
{
    // for testing only
    builder.Services.AddAuthentication("Test")
     .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
}
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
app.Seriloging();
// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.SwaggerConfigEx(builder.Configuration, app);
}
app.MigrateDatabase<SQLContext>((context, services) =>
{
    if (!app.Environment.IsProduction())
    { 
      // context.Database.EnsureDeleted(); 
       //context.Database.EnsureCreated();
        context.Database.Migrate();

    }

    var logger = services.GetService<ILogger<SqlDbContextSeed>>();
    SqlDbContextSeed.Seed(context);

    //SqlDbContextSeed.SeedAsync(context, logger).Wait();

}); 

app.AutoWrapperConfigEx();
//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();

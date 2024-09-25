using Microsoft.Extensions.Logging;

namespace DesignSphere.Infrastructure.Persistence;

public class AppDbContextSeed
{
    private static int records = 2;

    public static async Task SeedAsync(AppDbContext context, ILogger<AppDbContextSeed> logger)
    {

    }
}

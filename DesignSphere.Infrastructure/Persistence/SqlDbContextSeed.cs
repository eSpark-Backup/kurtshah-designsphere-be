using Microsoft.Extensions.Logging;
using DesignSphere.Infrastructure.Persistence;

namespace DesignSphere.Infrastructure.Persistence;
public class SqlDbContextSeed
{
    private static int records = 2;

    public static async Task SeedAsync(SQLContext context, ILogger<SqlDbContextSeed> logger)
    {

    }

}

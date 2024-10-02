namespace BuildingBlocks.Common.Models;

public class RedisSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public int Database { get; set; }
    public string InstanceName { get; set; }
}

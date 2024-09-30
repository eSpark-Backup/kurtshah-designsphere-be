using DesignSphere.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace DesignSphere.Infrastructure.Persistence;
public class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<UserComnpany> UserComnpanies { get; set; }
    public DbSet<Contarctor> Contarctors { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectPlan> ProjectPlans { get; set; }
    public DbSet<ProjectDetail> ProjectDetails { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<HardwareSet> HardwareSets { get; set; }
    public DbSet<HardwareCategory> HardwareCategories { get; set; }
    public DbSet<HardwareSeries> HardwaresSeries { get; set; }
    public DbSet<PowerSupply> PowerSupplies { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Door> Doors { get; set; }
    public DbSet<DoorFrame> DoorFrames { get; set; }
    public DbSet<DoorOpening> DoorOpenings{ get; set; }

}
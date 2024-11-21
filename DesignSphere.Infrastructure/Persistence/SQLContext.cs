using BuildingBlocks.Common.Services;
using DesignSphere.Domain.Common;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Convertors;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace DesignSphere.Infrastructure.Persistence;
public class SQLContext : DbContext 
{
    private readonly IUserService _userService; 

    public SQLContext(DbContextOptions<SQLContext> options, IUserService userService) : base(options)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().UseTpcMappingStrategy();
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToBytesConverter>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
        {
            string userid;
            string username;
            if (string.IsNullOrEmpty(entry.Entity.CreatorId))
            {
                userid = _userService.GetUserIdentity();
                username = _userService.GetUserName();
            }
            else
            {
                userid = entry.Entity.CreatorId;
                username = entry.Entity.CreatorName;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationTime = DateTime.Now;
                    entry.Entity.CreatorName = username;
                    entry.Entity.CreatorId = userid;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastmodificationTime = DateTime.Now;
                    entry.Entity.LastmodifierName = username;
                    entry.Entity.LastmodifierId = userid;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }


    public DbSet<Product> Products { get; set; }
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
    public DbSet<ProjectRole> ProjectRoles{ get; set; }
    public DbSet<ProjectUserRole> ProjectUserRoles{ get; set; }

}
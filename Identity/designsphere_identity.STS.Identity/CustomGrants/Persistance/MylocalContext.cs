using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

namespace designsphere_identity.STS.Identity.CustomGrants.Persistance;

public class MylocalContext : DbContext
{
    public MylocalContext(DbContextOptions<MylocalContext> options) : base(options)
    {
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationConfiguration> OrganizationConfigurations { get; set; }
    public DbSet<ExUserIdentity> ExUserIdentities { get; set; }
    public DbSet<UserDetail> UserDetails { get; set; }
    public DbSet<UserDetailLog> UserDetailLogs { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<JobRole> JobRole { get; set; }
    public DbSet<Country> Countries { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationTime = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastmodificationTime = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken).Result;
    }
}

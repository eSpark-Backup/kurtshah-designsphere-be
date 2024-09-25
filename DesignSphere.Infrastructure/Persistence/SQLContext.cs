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
    public DbSet<Comnpany> Comnpanies { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectPlan> ProjectPlans { get; set; }
    public DbSet<ProjectDetail> ProjectDetails { get; set; }
    public DbSet<ProjectCollaborator> ProjectCollaborators { get; set; }
    public DbSet<Building> Buildings { get; set; }

}
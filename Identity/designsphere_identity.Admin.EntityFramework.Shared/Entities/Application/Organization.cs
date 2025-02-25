using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
public class Organization
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Domain { get; set; }
    public string AdminUserId { get; set; }
    public string ConnectionString { get; set; } // Stores tenant database connection
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<UserIdentity> Users { get; set; }
}


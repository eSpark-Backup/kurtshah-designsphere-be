using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
public class Organization : EntityBase
{
    public string Name { get; set; }
    public string Domain { get; set; }
    public string SubDomain { get; set; }
    public string Website { get; set; }
    public string SupportEmailAddress { get; set; }
    public string AuthConnectionString { get; set; }
    public string ResourceConnectionString { get; set; }

    #region Navigation Properties
    public virtual ICollection<UserIdentity> Users { get; set; }
    public virtual ICollection<OrganizationConfiguration> OrganizationConfigurations { get; set; } 
    #endregion
}


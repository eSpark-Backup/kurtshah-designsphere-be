using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
public class OrganizationConfiguration : EntityBase
{
    [ForeignKey("OrganizationId")]
    public Guid OrganizationId { get; set; }
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
    public string ConfigType { get; set; }

    #region Navigation Property
    public Organization Organization { get; set; }
    #endregion
}

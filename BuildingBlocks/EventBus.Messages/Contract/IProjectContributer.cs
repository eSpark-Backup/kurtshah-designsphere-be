using SieveCustom.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IProjectContributer
{
     Ulid ContactId { get; set; }

     Ulid ProjectId { get; set; }

     Ulid ProjectRoleId { get; set; }

     bool IsContributerRemoved { get; set; } 

}
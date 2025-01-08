using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IProjectContributer
{
    string UserId { get; set; }
    ContributerRole ProjectRole { get; set; }
    Ulid ProjectId { get; set; }
    string Name { get; set; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface ITask
{
    Ulid ProjectId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    DateTime Deadline { get; set; }
    decimal TaskWeight { get; set; }
    ETaskStatus Status { get; set; }
}
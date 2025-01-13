using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface ITaskAssignee
{
    Ulid TaskStageId { get; set; }
    Ulid ContributerId { get; set; }
    TaskStatus Status { get; set; }
}
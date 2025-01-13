using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;

public interface ITaskDetail
{
    Ulid TaskId { get; set; }
    Ulid TaskStageId { get; set; }
     ETaskStatus Status { get; set; }
}
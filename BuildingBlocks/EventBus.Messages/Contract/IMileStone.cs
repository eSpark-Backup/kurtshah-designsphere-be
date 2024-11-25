using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IMileStone : IGeneric
{
    Ulid ProjectId { get; set; }
    DateTime DueDate { get; set; }
    EMileStoneStatus Status { get; set; }
    float Percentage { get; set; }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IDoor
{
    EDoorPanelType PanelType { get; set; }
    EDoorHand Hand { get; set; }
}
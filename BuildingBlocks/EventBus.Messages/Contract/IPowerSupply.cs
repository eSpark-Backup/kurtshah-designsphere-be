using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IPowerSupply
{
    float Current { get; set; }
    float VoltageString { get; set; }
    UInt16 Type { get; set; }

}
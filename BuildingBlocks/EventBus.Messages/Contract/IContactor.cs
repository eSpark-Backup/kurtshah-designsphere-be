using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IContactor
{
    string ContarctorName { get; set; }
   // Address Address { get; set; }
}
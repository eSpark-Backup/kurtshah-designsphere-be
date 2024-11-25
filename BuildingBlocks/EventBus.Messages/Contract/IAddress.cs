using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IAddress
{
    string CountryName { get; set; }
    ushort PostalCode { get; set; }
}
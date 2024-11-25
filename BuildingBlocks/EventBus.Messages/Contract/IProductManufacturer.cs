using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IProductManufacturer
{
    public Ulid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
}
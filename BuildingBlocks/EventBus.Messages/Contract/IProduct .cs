using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IProduct : IGeneric, ISize, IUnitPrice, IProductManufacturer
{
    Ulid MaterialId { get; set; }
    string MaterialDescription { get; set; }
}
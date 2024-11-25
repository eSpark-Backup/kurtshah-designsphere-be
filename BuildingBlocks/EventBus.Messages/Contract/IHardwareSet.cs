using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IHardwareSet
{
    Ulid HardwareCategoryId { get; set; }
    string HardwareCategoryDescription { get; set; }
    Ulid HardwareSeriesId { get; set; }
    string HardwareSeriesDescription { get; set; }
    Ulid PowerSupplyId { get; set; }
    Ulid PowerSupplyDescription { get; set; }

}
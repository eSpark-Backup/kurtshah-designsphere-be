using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface ISize
{
    float Width { get; set; }
    float Height { get; set; }
    float Thickness { get; set; }
}
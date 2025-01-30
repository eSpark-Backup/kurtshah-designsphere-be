using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Application.Contracts.Hubs;
public interface IBaseHub
{
    Task Test(string message);

}
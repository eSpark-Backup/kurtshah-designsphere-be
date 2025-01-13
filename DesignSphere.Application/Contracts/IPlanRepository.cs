using DesignSphere.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Application.Contracts;

public interface IPlanRepository : IAsyncRepository<ProjectPlan>
{
}
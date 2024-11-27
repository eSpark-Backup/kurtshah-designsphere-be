using DesignSphere.Domain.Entites;
using SieveCustom.Models;

namespace DesignSphere.Application.Contracts;

public interface IProjectRepository : IAsyncRepository<Project>
{
    Task<IReadOnlyList<Project>> GetProjectsAsync(SieveModel sieveModel);
}


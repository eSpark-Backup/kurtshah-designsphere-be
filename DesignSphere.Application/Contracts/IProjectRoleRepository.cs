using DesignSphere.Domain.Entites;

namespace DesignSphere.Application.Contracts;

public interface IProjectRoleRepository : IAsyncRepository<ProjectRole>
{
    Task<ProjectRole> getProjectRole(string roleName);

}


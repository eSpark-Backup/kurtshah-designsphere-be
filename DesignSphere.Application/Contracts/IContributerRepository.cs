using DesignSphere.Domain.Entites;


namespace DesignSphere.Application.Contracts;

public interface IContributerRepository : IAsyncRepository<ProjectContributer>
{
    Task<bool> getContributerByProject(Ulid contactId, Ulid projectId);

}

using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public interface IProjectDocumentRepository
    {
        Task<List<ProjectDocument>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<ProjectDocument>> GetAllAsync();
        Task<ProjectDocument?> GetByIdAsync(int id);
        Task AddAsync(ProjectDocument document);
        Task UpdateAsync(ProjectDocument document);
        Task DeleteAsync(ProjectDocument document);
        Task<bool> ExistsAsync(int id);
    }
}

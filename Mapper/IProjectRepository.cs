using human_resource_management.Model;
namespace human_resource_management.Mapper
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<bool> SaveChangesAsync();
    }
}

using human_resource_management.Model;
using System.Threading.Tasks;

namespace human_resource_management.Mapper
{
    public interface IPerformanceReviewRepository
    {
        Task<IEnumerable<PerformanceReview>> GetAllAsync();
        Task<IEnumerable<PerformanceReview>> GetByProjectIdAsync(int projectId);
        Task<PerformanceReview?> GetByIdAsync(int id);
        Task AddAsync(PerformanceReview review);
        Task UpdateAsync(PerformanceReview review);
        Task DeleteAsync(PerformanceReview review);
        Task<bool> ExistsAsync(int id);
    }
}

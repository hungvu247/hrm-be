using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public interface IRequestFormRepository
    {
        Task<IEnumerable<RequestForm>> GetAllAsync(int? employeeId);
        Task AddAsync(RequestForm requestForm);
        Task<bool> UpdateStatusAsync(int id, string status, int reviewedById);
    }
}

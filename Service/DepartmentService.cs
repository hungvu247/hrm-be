using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Service
{
    public class DepartmentService
    {
        private readonly Repository.DepartmentRepository _departmentRepository;
        public DepartmentService(Repository.DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<object> SearchPagedDepartmentsAsync(string? search, int page, int pageSize, string sort)
        {
            return await _departmentRepository.SearchPagedDepartmentsAsync(search, page, pageSize, sort);
        }
        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetDepartmentByIdAsync(id);
        }
        public async Task AddDepartmentAsync(Model.Department department)
        {
            await _departmentRepository.AddDepartmentAsync(department);
        }
        public async Task UpdateDepartmentAsync(Model.Department department)
        {
            await _departmentRepository.UpdateDepartmentAsync(department);
        }
        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteDepartmentAsync(id);
        }
    }
}

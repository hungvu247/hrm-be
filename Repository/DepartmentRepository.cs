using human_resource_management.Dto;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Repository
{
    public class DepartmentRepository
    {
        private readonly HumanResourceManagementContext _context;
        public DepartmentRepository(HumanResourceManagementContext context)
        {
            _context = context;
        }
        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
                .ToListAsync();

            return departments.Select(dept => new DepartmentDto
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
                Description = dept.Description,
                Employees = dept.Employees.Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FirstName + " " + e.LastName,
                    Position = e.Position?.PositionName
                }).ToList()
            }).ToList();
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            var dept = await _context.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Position) // nếu cần
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (dept == null) return null;

            return new DepartmentDto
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
                Description = dept.Description,
                Employees = dept.Employees.Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FirstName + " " + e.LastName,
                    Position = e.Position?.PositionName
                }).ToList()
            };
        }


        public async Task AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDepartmentAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }
        public async Task<Department?> GetDepartmentEntityByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees) // nếu cần kiểm tra quan hệ trước khi xóa
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await GetDepartmentEntityByIdAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
    }
}

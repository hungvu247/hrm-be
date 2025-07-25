using System;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;
namespace human_resource_management.Repository
{
    public class ProjectRepository : Mapper.IProjectRepository
    {
        private readonly HumanResourceManagementContext _context;

        public ProjectRepository(HumanResourceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetPagedProjectsAsync(int? employeeId, int page, int pageSize, string? search)
        {
            if (employeeId == null)
                return new List<Project>();

            // Lấy employee hiện tại
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return new List<Project>();

            IQueryable<Project> query;

            if (employee.RoleId == 6 || employee.RoleId == 2)
            {
                query = _context.Projects.AsQueryable();
            }
            else
            {
                // Chỉ xem được các project mà họ được phân công
                query = _context.Projects
                    .Where(p => p.EmployeeProjects.Any(ep => ep.EmployeeId == employeeId))
                    .AsQueryable();
            }

            // Áp dụng điều kiện tìm kiếm nếu có
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProjectName.Contains(search));
            }

            // Phân trang
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Project>> GetAllAsync() =>
            await _context.Projects.ToListAsync();

        public async Task<Project?> GetByIdAsync(int id) =>
            await _context.Projects.FindAsync(id);

        public async Task<Project> AddAsync(Project project)
        {
            // Kiểm tra StartDate và EndDate không được null
            if (project.StartDate == null || project.EndDate == null)
            {
                throw new ArgumentException("StartDate và EndDate không được để trống.");
            }

            // Kiểm tra ProjectName không trùng
            var existingProject = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectName.ToLower() == project.ProjectName.ToLower());

            if (existingProject != null)
            {
                throw new InvalidOperationException($"Dự án với tên '{project.ProjectName}' đã tồn tại.");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateAsync(Project project)
        {

            // Kiểm tra StartDate và EndDate không được null
            if (project.StartDate == null || project.EndDate == null)
            {
                throw new ArgumentException("StartDate và EndDate không được để trống.");
            }

            // Kiểm tra ProjectName không trùng với project khác
            var existingProject = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectName.ToLower() == project.ProjectName.ToLower()
                                          && p.ProjectId != project.ProjectId);

            if (existingProject != null)
            {
                throw new InvalidOperationException($"Tên dự án '{project.ProjectName}' đã tồn tại.");
            }

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
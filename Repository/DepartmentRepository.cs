﻿using human_resource_management.Dto;
using human_resource_management.Model;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<object> SearchPagedDepartmentsAsync(string search, int skip, int top, string orderBy)
        {
            var query = _context.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
                .AsQueryable();
            query = query.Where(d => d.Status == "Active");
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.DepartmentName.Contains(search));
            }

            var totalCount = await query.CountAsync();

            // Sắp xếp (mặc định theo DepartmentName)
            query = orderBy switch
            {
                "DepartmentName" => query.OrderBy(d => d.DepartmentName),
                "Description" => query.OrderBy(d => d.Description),
                _ => query.OrderBy(d => d.DepartmentId)
            };

            var paged = await query.Skip(skip).Take(top).ToListAsync();
            var items = paged.Select(dept => new DepartmentDto
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
                Description = dept.Description,
                LeadEmployee = dept.LeadEmployee != null ? new EmployeeDto
                {
                    EmployeeId = dept.LeadEmployee.EmployeeId,
                    FullName =dept.LeadEmployee.LastName,
                    Position = dept.LeadEmployee.Position?.PositionName ?? "No Position"
                } : null, 
                Employees = dept.Employees?.Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Position = e.Position?.PositionName ?? "No Position"
                }).ToList() ?? new List<EmployeeDto>() // Ensure that EmployeeDto is used here too
            }).ToList();


            return new
            {
                TotalCount = totalCount,
                Items = items,
                PageSize = top,
                Page = (skip / top) + 1
            };
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
                }).ToList(),
                LeadEmployee = dept.LeadEmployee != null ? new EmployeeDto
                {
                    EmployeeId = dept.LeadEmployee.EmployeeId,
                    FullName = dept.LeadEmployee.LastName,
                    Position = dept.LeadEmployee.Position?.PositionName ?? "No Position"
                } : null,
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
                department.Status = "Inactive"; // Giả sử bạn muốn đánh dấu là không hoạt động thay vì xóa
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();

               
                    }
        }
    }
}

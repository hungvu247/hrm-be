using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Service;
using human_resource_management.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public EmployeeController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("get-employee-by-id/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            var dto = EmployeeMapper.ToDto(employee);

            return Ok(dto);
        }

        [Produces("application/json")]
        [HttpPost("get-all-employee")]
        public async Task<IActionResult> SearchEmployees(
            [FromBody] SearchEmployeeDto searchDto,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Role)
                .AsQueryable();

            // ✅ Apply điều kiện lọc trước khi Count
            if (!string.IsNullOrEmpty(searchDto.Keyword))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(searchDto.Keyword) ||
                    e.LastName.Contains(searchDto.Keyword) ||
                    e.Email.Contains(searchDto.Keyword) ||
                    e.PhoneNumber.Contains(searchDto.Keyword) ||
                    e.Address.Contains(searchDto.Keyword));
            }

            if (searchDto.DepartmentId.HasValue)
                query = query.Where(e => e.DepartmentId == searchDto.DepartmentId);

            if (searchDto.PositionId.HasValue)
                query = query.Where(e => e.PositionId == searchDto.PositionId);

            if (searchDto.SalaryMin.HasValue)
                query = query.Where(e => e.Salary >= searchDto.SalaryMin);

            if (searchDto.SalaryMax.HasValue)
                query = query.Where(e => e.Salary <= searchDto.SalaryMax);

            if (searchDto.HireDateMin.HasValue)
                query = query.Where(e => e.HireDate >= searchDto.HireDateMin);

            if (searchDto.HireDateMax.HasValue)
                query = query.Where(e => e.HireDate <= searchDto.HireDateMax);

            // ✅ Tính đúng tổng bản ghi sau khi đã lọc
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // ✅ Phân trang
            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = employees.Select(EmployeeMapper.ToDto).ToList();

            return Ok(new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalPages = totalPages,
                totalRecords = totalRecords,
                data = result
            });
        }



        [HttpPost("add-employee")]
        [Produces("application/json")]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEmployee = EmployeeMapper.ToEntity(dto);

            if (!string.IsNullOrWhiteSpace(newEmployee.Password))
            {
                newEmployee.Password = PasswordHasher.HashPassword(newEmployee.Password);
            }
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, null);
        }

        [HttpPut("update-employee/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound($"Không tìm thấy nhân viên với ID = {id}");

            // Cập nhật từng field nếu có dữ liệu mới
            if (!string.IsNullOrWhiteSpace(dto.FirstName)) employee.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName)) employee.LastName = dto.LastName;
            if (dto.DateOfBirth.HasValue) employee.DateOfBirth = dto.DateOfBirth.Value;
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber)) employee.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(dto.Address)) employee.Address = dto.Address;
            if (dto.HireDate.HasValue) employee.HireDate = dto.HireDate.Value;
            if (dto.Salary.HasValue) employee.Salary = dto.Salary;
            if (!string.IsNullOrWhiteSpace(dto.Status)) employee.Status = dto.Status;
            if (dto.PositionId.HasValue) employee.PositionId = dto.PositionId;
            if (dto.DepartmentId.HasValue) employee.DepartmentId = dto.DepartmentId;
            //if (!string.IsNullOrWhiteSpace(dto.Username)) employee.Username = dto.Username;
            if (!string.IsNullOrWhiteSpace(dto.Email)) employee.Email = dto.Email;
            if (dto.RoleId.HasValue) employee.RoleId = dto.RoleId;

            // Mã hóa mật khẩu nếu có cập nhật
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                employee.Password = PasswordHasher.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok($"Cập nhật nhân viên ID {id} thành công.");
        }



    }
}

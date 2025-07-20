using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProjectController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public EmployeeProjectController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-employee2")]
        [Produces("application/json")]
        public IActionResult GetAllEmployee2()
        {
            var employees = _context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.Username

                }).ToList();
            return Ok(employees);
        }


        [HttpPost("get-by-project-id")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByProjectId([FromBody] SearchEmployeeProjectDto payload)
        {
            // Bắt đầu với kiểu IQueryable để dễ dàng thêm điều kiện
            var query = _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .Where(ep => ep.ProjectId == payload.ProjectId)
                .AsQueryable();

            // Thêm điều kiện search theo keyword nếu có
            if (!string.IsNullOrEmpty(payload.Keyword))
            {
                var keywordLower = payload.Keyword.ToLower();
                query = query.Where(ep =>
                    ep.Employee.Username.ToLower().Contains(keywordLower) ||
                    ep.Employee.Email.ToLower().Contains(keywordLower)
                );
            }

            // Thêm điều kiện theo EmployeeId nếu có
            if (payload.EmployeeId.HasValue)
            {
                query = query.Where(ep => ep.EmployeeId == payload.EmployeeId.Value);
            }

            var totalCount = await query.CountAsync();

            var employeeProjects = await query
                .Skip((payload.Page - 1) * payload.PageSize)
                .Take(payload.PageSize)
                .ToListAsync();

            if (!employeeProjects.Any())
            {
                return Ok(new { message = "No employees found for this project." });
            }

            var result = employeeProjects.Select(ep => ProjectEmployeeMapper.ToDto(ep)).ToList();

            return Ok(new
            {
                totalCount,
                payload.Page,
                payload.PageSize,
                data = result
            });
        }

        [HttpPost("add")]
        [Produces("application/json")]
        public async Task<IActionResult> Add([FromBody] AddProjectEmployeeDto dto)

        {
            var check = _context.EmployeeProjects.
                FirstOrDefaultAsync(ep => ep.EmployeeId == dto.EmployeeId && ep.ProjectId == dto.ProjectId);
            if (check.Result != null)
            {
                return Ok(new
                {
                    message = "This employee is already assigned to this project.",
                    StatusCode = 5000
                });
            }
            var entity = ProjectEmployeeMapper.ToEntity2(dto);

            _context.EmployeeProjects.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Added successfully" });
        }


        [HttpPut("update")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromBody] AddProjectEmployeeDto dto)
        {
            var entity = await _context.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == dto.EmployeeId && ep.ProjectId == dto.ProjectId);

            if (entity == null)
            {
                return NotFound(new { message = "Not found" });
            }

            entity.RoleInProject = dto.RoleInProject;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("delete")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete([FromBody] AddProjectEmployeeDto dto)
        {
            var entity = await _context.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == dto.EmployeeId && ep.ProjectId == dto.ProjectId);

            if (entity == null)
            {
                return NotFound(new { message = "Not found" });
            }

            _context.EmployeeProjects.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted successfully" });
        }


        [HttpGet("get-all-project")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllProject()
        {
            var employeeProjects = await _context.Projects
               .Select(p => new
               {
                   ProjectId = p.ProjectId,
                   ProjectName = p.ProjectName
               })
                .ToListAsync();

            return Ok(employeeProjects);
        }

        [HttpGet("get-all-employee")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employeeProjects = await _context.Employees
               .Select(p => new
               {
                   employeeId = p.EmployeeId,
                   name = p.Username
               })
                .ToListAsync();

            return Ok(employeeProjects);
        }

        [HttpGet("get-project-by-id/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = _context.Projects
                .Where(p => p.ProjectId == id)
                .Select(p => new
                {
                    p.ProjectId,
                    p.ProjectName
                })
                .FirstOrDefault();
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpDelete("delete/{employeeId}/{projectId}")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int employeeId, int projectId)
        {
            var e = _context.EmployeeProjects
                .Where(e => e.EmployeeId == employeeId && e.ProjectId == projectId)
                .FirstOrDefault();
            if (e != null)
            {
                _context.EmployeeProjects.Remove(e);
                _context.SaveChanges();
            }
            else
            {
                return NotFound("Không tồn tại");
            }
            return Ok();
        }

        [HttpGet("get-to-edit/{employeeId}/{projectId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByProjectIdAndEmployeeId(int employeeId, int projectId)
        {
            var e = _context.EmployeeProjects
                .Include(e => e.Project)
                .Include(e => e.Employee)
                .Where(e => e.EmployeeId == employeeId && e.ProjectId == projectId)
                .Select(e => new
                {
                    e.ProjectId,
                    e.EmployeeId,
                    e.Project.ProjectName,
                    UserName = e.Employee.Username,
                    e.RoleInProject
                })
                .FirstOrDefault();
            if (e != null)
            {

                return Ok(e);
            }
            else
            {
                return NotFound("Không tồn tại");
            }

        }
    }
}
using human_resource_management.Dto;
using human_resource_management.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public StatisticsController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("employee-by-department")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<DepartmentEmployeeStats>>> GetEmployeeStatsByDepartment()
        {
            var stats = await _context.Employees
      .Where(e => e.Department != null)
      .GroupBy(e => new { e.DepartmentId, e.Department.DepartmentName })
      .Select(g => new DepartmentEmployeeStats
      {
          DepartmentId = g.Key.DepartmentId ?? 0,
          DepartmentName = g.Key.DepartmentName,
          EmployeeCount = g.Count()
      })
      .OrderBy(s => s.DepartmentName)
      .ToListAsync();


            return Ok(stats);
        }
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PositionStats>>> GetPositionStats()
        {
            var stats = await _context.Employees
                .GroupBy(e => e.Position.PositionName)
                .Select(g => new PositionStats
                {
                    PositionName = g.Key,
                    EmployeeCount = g.Count()
                })
                .ToListAsync();

            return Ok(stats);
        }
        [HttpGet("{projectId}")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PerformanceStats>>> GetPerformanceStatsByProject(int projectId)
        {
            var stats = await _context.PerformanceReviews
                .Include(pr => pr.Employee)
                .Include(pr => pr.Project)
                .Where(pr => pr.Project.ProjectId == projectId)
                .Select(pr => new PerformanceStats
                {
                    ProjectName = pr.Project.ProjectName,
                    EmployeeName = pr.Employee.FirstName + " " + pr.Employee.LastName,
                    Score = pr.Score,
                    ReviewDate = pr.ReviewDate
                })
                .OrderBy(pr => pr.ReviewDate)
                .ToListAsync();

            return Ok(stats);
        }
    }

}

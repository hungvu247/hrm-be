using human_resource_management.Dto;
using human_resource_management.Model;
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

    }

}

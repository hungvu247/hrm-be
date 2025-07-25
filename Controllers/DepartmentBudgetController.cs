using human_resource_management.Dto;
using human_resource_management.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentBudgetController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public DepartmentBudgetController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        // Lấy dữ liệu ngân sách phòng ban
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetDepartmentBudgets()
        {
            var departmentBudgets = await _context.DepartmentBudgets
                .Include(db => db.Department)  // Join với bảng Departments
                .OrderBy(db => db.Department.DepartmentName)
                .ToListAsync();

            // Chuyển đổi dữ liệu sang DTO
            var departmentBudgetDTOs = departmentBudgets.Select(db => new DepartmentBudgetDTO
            {
                BudgetId = db.BudgetId,
                DepartmentName = db.Department.DepartmentName,
                AllocatedBudget = db.AllocatedBudget,
                UsedBudget = db.UsedBudget,
                Year = db.Year
            }).ToList();

            return Ok(departmentBudgetDTOs);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    // Bảo vệ API bằng xác thực
    public class DepartmentController : ControllerBase
    {
        private readonly Service.DepartmentService _departmentService;
        public DepartmentController(Service.DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetDepartments(
     string search = "",
     int skip = 0,
     int top = 10,
     string orderBy = "DepartmentName"
 )
        {
            var result = await _departmentService.SearchPagedDepartmentsAsync(search, skip, top, orderBy);
            return Ok(result);
        }



        [HttpGet("{id}")]
        [Produces("application/json")] // Bắt buộc trả JSON
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var dept = await _departmentService.GetDepartmentByIdAsync(id);
            if (dept == null)
                return NotFound();
            return Ok(dept);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> AddDepartment([FromBody] Model.Department department)
        {
            if (department == null)
            {
                return BadRequest();
            }
            await _departmentService.AddDepartmentAsync(department);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentId }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Model.Department department)
        {
            if (id != department.DepartmentId || department == null)
            {
                return BadRequest();
            }
            await _departmentService.UpdateDepartmentAsync(department);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }

    }
}
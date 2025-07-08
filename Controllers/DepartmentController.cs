using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize]
   // Bảo vệ API bằng xác thực
    public class DepartmentController : ControllerBase
    {
        private readonly Service.DepartmentService _departmentService;
        public DepartmentController(Service.DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet()]
        [Produces("application/json")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments); // đảm bảo trả về JSON
        }

        [HttpGet("get-all-department")]
        [Produces("application/json")]
        public async Task<IActionResult> GetDepartments2()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            var result = departments.Select(d => new
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Description = d.Description
            });
            return Ok(result); // đảm bảo trả về JSON
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

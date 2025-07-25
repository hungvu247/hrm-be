using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    //  Bảo vệ API bằng xác thực
    public class DepartmentController : ControllerBase
    {
        private readonly Service.DepartmentService _departmentService;
        public DepartmentController(Service.DepartmentService departmentService)
        {
            _departmentService = departmentService;
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
            return Ok(result);
        }

        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "admin")]
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
        [Produces("application/json")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var dept = await _departmentService.GetDepartmentByIdAsync(id);
            if (dept == null)
                return NotFound();
            return Ok(dept);
        }

        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateDepartmentStatus(int id, [FromBody] JsonElement body)
        {
            if (!body.TryGetProperty("status", out var statusElement))
            {
                return BadRequest("Missing 'status' field.");
            }

            string status = statusElement.GetString() ?? "Inactive";


            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }


            department.Status = status;

            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }


    }
}
using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public RoleController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-role")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllRole()
        {
            try
            {
                var roles = await _context.Roles
                    .Select(r => RoleMapper.ToDto(r))
                    .ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving positions.", error = ex.Message });
            }
        }
        [Produces("application/json")]
        [HttpGet("get-role-by-id/{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var role = await _context.Roles
                .Where(r => r.RoleId == id)
                .Select(r => RoleMapper.ToDto(r))
                .FirstOrDefaultAsync();

            if (role == null)
                return NotFound();
            return Ok(role);
        }
    }
}
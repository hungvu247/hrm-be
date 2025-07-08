using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public PositionController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-position")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllPosition()
        {
            try
            {
                var positions = await _context.Positions
                    .Select(p => PositionMapper.ToDto(p))
                    .ToListAsync();
                return Ok(positions); // đảm bảo trả về JSON
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving positions.", error = ex.Message });
            }
        }
        [Produces("application/json")]
        [HttpGet("get-position-by-id/{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var position = await _context.Positions
                .Where(p => p.PositionId == id)
                .Select(p => PositionMapper.ToDto(p))
                .FirstOrDefaultAsync();

            if (position == null)
                return NotFound();

            return Ok(position);
        }



    }
}

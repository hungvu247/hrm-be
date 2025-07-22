using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "HR")]
        public IActionResult GetPositionTree(string search = "",
                                             int skip = 0,
                                             int top = 10,
                                             string orderBy = "DepartmentName")
        {
            var positions = _context.Positions
                .Include(p => p.Employees)
                .ToList();
            if (!string.IsNullOrEmpty(search))
            {
                positions = positions
                    .Where(p => p.PositionName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            var totalCount = positions.Count;

            positions = orderBy switch
            {
                "PositionName" => positions.OrderBy(p => p.PositionName).ToList(),
                _ => positions.OrderBy(p => p.PositionId).ToList()
            };
            var pagedPositions = positions.Skip(skip).Take(top).ToList();
            var item = pagedPositions.Select(p => new PositionTreeDto
            {
                PositionId = p.PositionId,
                PositionName = p.PositionName,
                Employees = p.Employees.Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FirstName + " " + e.LastName
                }).ToList()
            });
            return Ok(new
            {
                TotalCount = totalCount,
                Items = item,
                PageSize = top,
                Page = (skip / top) + 1
            });
        }
        [HttpGet("{id}")]
        [Produces("application/json")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetPositionById1(int id)
        {
            var position = await _context.Positions
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }
            var positionDto = new PositionTreeDto
            {
                PositionId = position.PositionId,
                PositionName = position.PositionName,
                Employees = position.Employees.Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FirstName + " " + e.LastName
                }).ToList()
            };
            return Ok(positionDto);
        }
        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> AddPosition([FromBody] Position position)
        {
            if (position == null || string.IsNullOrWhiteSpace(position.PositionName))
            {
                return BadRequest(new { message = "Thông tin vị trí không hợp lệ" });
            }

            bool isDuplicate = await _context.Positions
                .AnyAsync(p => p.PositionName.ToLower() == position.PositionName.ToLower());

            if (isDuplicate)
            {
                return Conflict(new { message = "Tên vị trí đã tồn tại" });
            }

            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPositionById), new { id = position.PositionId }, position);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] Position position)
        {
            if (id != position.PositionId || position == null)
            {
                return BadRequest();
            }
            if (position == null || string.IsNullOrWhiteSpace(position.PositionName))
            {
                return BadRequest(new { message = "Thông tin vị trí không hợp lệ" });
            }

            bool isDuplicate = await _context.Positions
      .AnyAsync(p => p.PositionName.ToLower() == position.PositionName.ToLower()
                  && p.PositionId != position.PositionId);


            if (isDuplicate)
            {
                return Conflict(new { message = "Tên vị trí đã tồn tại" }); 
            }
            _context.Entry(position).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return NoContent();
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
                return Ok(positions); 
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

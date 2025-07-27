using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Repository;
using human_resource_management.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestFormRepository _repository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public RequestController(IRequestFormRepository repository, IMapper mapper, JwtService jwtService  )
        {
            _repository = repository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Produces("application/json")]

        public async Task<IActionResult> GetByEmployee()
        {

            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var employeeId = _jwtService.GetEmployeeIdFromToken(accessToken);
            var result = await _repository.GetAllAsync(employeeId);
            return Ok(_mapper.Map<IEnumerable<RequestFormDto>>(result));
        }
        [HttpPost]
        [Produces("application/json")]

        public async Task<IActionResult> Create([FromBody] CreateRequestFormDto dto)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var employeeId = _jwtService.GetEmployeeIdFromToken(accessToken);
                if (employeeId == null) return BadRequest();

                var requestForm = _mapper.Map<RequestForm>(dto);
                
                requestForm.EmployeeId = (int)employeeId;
                requestForm.Status = "Pending";
                requestForm.CreatedAt = DateTime.Now;

                await _repository.AddAsync(requestForm);
                return Ok(new { message = "Request created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-status/{id}")]
        [Produces("application/json")]
        [Authorize(Roles = "manager,Lead,HR")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            // ✅ Lấy EmployeeId từ token nếu có xác thực JWT
            var employeeIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId");
            if (employeeIdClaim == null) return Unauthorized();

            int reviewedById = int.Parse(employeeIdClaim.Value);

            var success = await _repository.UpdateStatusAsync(id, dto.Status, reviewedById);
            if (!success) return NotFound();

            return Ok();
        }




        //// GET: api/request/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<RequestFormDto>> GetById(int id)
        //{
        //    var form = await _repository.GetByIdAsync(id);
        //    if (form == null) return NotFound();
        //    return Ok(_mapper.Map<RequestFormDto>(form));
        //}

        //// POST: api/request
        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] RequestFormDto dto)
        //{
        //    var entity = _mapper.Map<RequestForm>(dto);
        //    await _repository.AddAsync(entity);
        //    return CreatedAtAction(nameof(GetById), new { id = entity.RequestId }, entity);
        //}

        //// PUT: api/request/{id}
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update(int id, [FromBody] RequestFormDto dto)
        //{
        //    var existing = await _repository.GetByIdAsync(id);
        //    if (existing == null) return NotFound();

        //    _mapper.Map(dto, existing);
        //    await _repository.UpdateAsync(existing);
        //    return NoContent();
        //}

        //// DELETE: api/request/{id}
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var existing = await _repository.GetByIdAsync(id);
        //    if (existing == null) return NotFound();

        //    await _repository.DeleteAsync(existing);
        //    return NoContent();
        //}
    }
}

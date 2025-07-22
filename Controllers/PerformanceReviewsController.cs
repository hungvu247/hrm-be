using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceReviewsController : ControllerBase
    {
        private readonly IPerformanceReviewRepository _repo;
        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _mapper;
        private readonly HumanResourceManagementContext _context;


        public PerformanceReviewsController(
            IPerformanceReviewRepository repo,
            IProjectRepository projectRepo,
            HumanResourceManagementContext context,
            IMapper mapper)
        {
            _repo = repo;
            _projectRepo = projectRepo;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]

        public async Task<ActionResult<PerformanceReviewReadDto>> GetAll()
        {
            var reviews = await _repo.GetAllAsync();
            return Ok(_mapper.Map<PerformanceReviewReadDto>(reviews));
        }
        [HttpGet("project/{projectId}")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PerformanceReviewReadDto>>> GetByProjectId(int projectId)
        {
            var reviews = await _repo.GetByProjectIdAsync(projectId);
            if (!reviews.Any()) return NotFound($"No reviews found for project with ID {projectId}");

            return Ok(_mapper.Map<IEnumerable<PerformanceReviewReadDto>>(reviews));
        }

        [HttpGet("{id}")]
        [Produces("application/json")]

        public async Task<ActionResult<PerformanceReviewReadDto>> GetById(int id)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(_mapper.Map<PerformanceReviewReadDto>(review));
        }

        [HttpPost]
        [Produces("application/json")]

        public async Task<ActionResult<PerformanceReviewReadDto>> Create(PerformanceReviewCreateDto dto)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId);

            if (!employeeExists) 
                return BadRequest($"EmployeeId {dto.EmployeeId} does not exist.");

            if (await _projectRepo.GetByIdAsync(dto.ProjectId) == null)
                return BadRequest($"ProjectId {dto.ProjectId} does not exist.");

            var review = _mapper.Map<PerformanceReview>(dto);
            await _repo.AddAsync(review);

            var result = _mapper.Map<PerformanceReviewReadDto>(review);
            return CreatedAtAction(nameof(GetById), new { id = result.ReviewId }, result);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> Update(int id, PerformanceReviewUpdateDto dto)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            _mapper.Map(dto, review);
            await _repo.UpdateAsync(review);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            await _repo.DeleteAsync(review);
            return NoContent();
        }
    }
}

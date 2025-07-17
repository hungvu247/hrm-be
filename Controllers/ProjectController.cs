using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Repository;
using human_resource_management.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetAll()
        {
            var projects = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectReadDto>>(projects));
        }

        [HttpGet("{id}")]
        [Produces("application/json")]

        public async Task<ActionResult<ProjectReadDto>> GetById(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            return Ok(_mapper.Map<ProjectReadDto>(project));
        }

        [HttpPost]
        [Produces("application/json")]

        public async Task<ActionResult<ProjectReadDto>> Create(ProjectCreateDto dto)
        {
            var project = _mapper.Map<Project>(dto);

            await _repo.AddAsync(project); // EF Core tự sinh Id

            var readDto = _mapper.Map<ProjectReadDto>(project);

            return CreatedAtAction(nameof(GetById), new { id = readDto.ProjectId }, readDto);
        }


        [HttpPut("{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> Update(int id, ProjectUpdateDto dto)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            _mapper.Map(dto, project);
            await _repo.UpdateAsync(project);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            await _repo.DeleteAsync(project);
            return NoContent();
        }



    }
}
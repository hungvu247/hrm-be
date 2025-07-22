using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDocumentsController : ControllerBase
    {
        private readonly IProjectDocumentRepository _repo;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _repoPro;

        public ProjectDocumentsController(IProjectDocumentRepository repo, IMapper mapper, IProjectRepository repoPro)
        {
            _repo = repo;
            _mapper = mapper;
            _repoPro = repoPro;
        }

        [HttpGet("document/{projectId}")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ProjectDocumentReadDto>>> GetByProjectId(int projectId)
        {
            var documents = await _repo.GetByProjectIdAsync(projectId);
            if (documents == null || !documents.Any())
                return NotFound($"No documents found for ProjectId = {projectId}");

            var documentDtos = _mapper.Map<IEnumerable<ProjectDocumentReadDto>>(documents);
            return Ok(documentDtos);
        }
        [HttpGet]
        [Produces("application/json")]

        public async Task<ActionResult<IEnumerable<ProjectDocumentReadDto>>> GetAll()
        {
            var documents = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectDocumentReadDto>>(documents));
        }


        [HttpGet("{id}")]
        [Produces("application/json")]

        public async Task<ActionResult<ProjectDocumentReadDto>> GetById(int id)
        {
            var document = await _repo.GetByIdAsync(id);
            if (document == null) return NotFound();
            return Ok(_mapper.Map<ProjectDocumentReadDto>(document));
        }

        [HttpPost]
        [Produces("application/json")]

        public async Task<ActionResult<ProjectDocumentReadDto>> Create([FromBody] ProjectDocumentCreateDto dto)
        {
            var projectExists = await _repoPro.GetByIdAsync(dto.ProjectId);
            if (projectExists == null)
            {
                return BadRequest($"Project with ID {dto.ProjectId} does not exist.");
            }

            var document = _mapper.Map<ProjectDocument>(dto);
            await _repo.AddAsync(document);
            var result = _mapper.Map<ProjectDocumentReadDto>(document);
            return CreatedAtAction(nameof(GetById), new { id = result.DocumentId }, result);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> Update(int id, [FromBody] ProjectDocumentUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var document = await _repo.GetByIdAsync(id);
            if (document == null) return NotFound();

            await _repo.DeleteAsync(document);
            return NoContent();
        }
    }
}

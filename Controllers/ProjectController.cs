using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Repository;
using human_resource_management.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public ProjectController(IProjectRepository repo, IMapper mapper, JwtService jwtService)
        {
            _repo = repo;
            _mapper = mapper;
            _jwtService = jwtService;
        }
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "HR,Lead,manager")]
        public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetProjects(int page = 1, int pageSize = 10, string? search = null)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var employeeId = _jwtService.GetEmployeeIdFromToken(accessToken);

            if (employeeId == null)
            {
                return Unauthorized("EmployeeId is missing or invalid in token.");
            }
            var projects = await _repo.GetPagedProjectsAsync(employeeId, page, pageSize, search);
            var projectDtos = _mapper.Map<IEnumerable<ProjectReadDto>>(projects);
            return Ok(projectDtos);
        }

        //[HttpGet]
        //[Produces("application/json")]
        //public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetAll()
        //{
        //    var projects = await _repo.GetAllAsync();
        //    return Ok(_mapper.Map<IEnumerable<ProjectReadDto>>(projects));
        //}

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
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<ProjectReadDto>> Create([FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var project = _mapper.Map<Project>(dto);

                await _repo.AddAsync(project); // Repo đã kiểm tra logic

                var readDto = _mapper.Map<ProjectReadDto>(project);

                return CreatedAtAction(nameof(GetById), new { id = readDto.ProjectId }, readDto);
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi tên dự án đã tồn tại
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Lỗi ngày không hợp lệ
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi không xác định
                return StatusCode(500, new { message = "Đã xảy ra lỗi trên server.", detail = ex.Message });
            }
        }



        [HttpPut("{id}")]
        [Produces("application/json")]
        [Authorize(Roles = "manager")]
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
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            await _repo.DeleteAsync(project);
            return NoContent();
        }



    }
}
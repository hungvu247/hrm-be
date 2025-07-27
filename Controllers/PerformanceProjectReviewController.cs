using human_resource_management.Dto;
using human_resource_management.IService;
using human_resource_management.Model;
using human_resource_management.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceProjectReviewController : ControllerBase
    {

        private readonly HumanResourceManagementContext _context;
        public PerformanceProjectReviewController(HumanResourceManagementContext context)
        {
            _context = context;
        }
        [HttpGet("statistics")]
        [Produces("application/json")]
        public IActionResult GetProjectReviewStatistics()
        {
            var statistics = _context.PerformanceReviews
             .GroupBy(pr => pr.ProjectId)
             .Select(g => new ProjectReviewStatisticDto
             {
                 ProjectId = g.Key,
                 ProjectName = _context.Projects
                                   .Where(p => p.ProjectId == g.Key)
                                   .Select(p => p.ProjectName)
                                   .FirstOrDefault(),
                 TotalReviews = g.Count(),
                 AverageScore = g.Average(pr => pr.Score) ?? 0,
                 MaxScore = g.Max(pr => pr.Score) ?? 0,
                 MinScore = g.Min(pr => pr.Score) ?? 0,
                 Reviews = g.Select(pr => new ReviewDetailDto
                 {
                     ReviewId = pr.ReviewId,
                     EmployeeId = pr.EmployeeId,
                     Username = _context.Employees
                                   .Where(e => e.EmployeeId == pr.EmployeeId)
                                   .Select(e => e.Username)
                                   .FirstOrDefault(),
                     Score = pr.Score ?? 0,
                     ReviewDate = pr.ReviewDate,
                     Comments = pr.Comments
                 }).ToList()
             })
             .ToList();




            return Ok(statistics);
        }

        [HttpGet("projects")]
        [Produces("application/json")]
        public IActionResult GetAllProjects()
        {
            var projects = _context.Projects
                .Select(p => new
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName
                })
                .ToList();

            return Ok(projects);
        }


        [Produces("application/json")]
        [HttpGet("statistics/employee-average")]
        public IActionResult GetEmployeeAverageScorePerProject([FromQuery] int projectId)
        {
            var data = _context.PerformanceReviews
                .Where(pr => pr.ProjectId == projectId)
                .GroupBy(pr => pr.EmployeeId)
                .Select(g => new EmployeeAverageScoreDto
                {
                    EmployeeId = g.Key,
                    Username = _context.Employees
                                .Where(e => e.EmployeeId == g.Key)
                                .Select(e => e.Username)
                                .FirstOrDefault(),
                    ProjectId = projectId,
                    ProjectName = _context.Projects
                                .Where(p => p.ProjectId == projectId)
                                .Select(p => p.ProjectName)
                                .FirstOrDefault(),
                    AverageScore = g.Average(x => x.Score) ?? 0,
                    TotalReviews = g.Count()
                })
                .ToList();

            return Ok(data);
        }



        [HttpGet("statistics/top5-employees")]
        [Produces("application/json")]
        public IActionResult GetTop5Employees()
        {
            var data = _context.PerformanceReviews
                .GroupBy(pr => pr.EmployeeId)
                .Select(g => new TopEmployeeDto
                {
                    EmployeeId = g.Key,
                    Username = _context.Employees
                                .Where(e => e.EmployeeId == g.Key)
                                .Select(e => e.Username)
                                .FirstOrDefault(),
                    AverageScore = g.Average(x => x.Score) ?? 0
                })
                .OrderByDescending(x => x.AverageScore)
                .Take(5)
                .ToList();

            return Ok(data);
        }



        [HttpGet("statistics/employee-review-count")]
        [Produces("application/json")]
        public IActionResult GetEmployeeReviewCount()
        {
            var data = _context.PerformanceReviews
                .GroupBy(pr => pr.EmployeeId)
                .Select(g => new EmployeeReviewCountDto
                {
                    EmployeeId = g.Key,
                    Username = _context.Employees
                                .Where(e => e.EmployeeId == g.Key)
                                .Select(e => e.Username)
                                .FirstOrDefault(),
                    TotalReviews = g.Count()
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("statistics/role-average")]
        [Produces("application/json")]

        public IActionResult GetRoleAverageScorePerProject()
        {
            var data = _context.PerformanceReviews
                .Join(_context.EmployeeProjects,
                    pr => new { pr.EmployeeId, pr.ProjectId },
                    ep => new { ep.EmployeeId, ep.ProjectId },
                    (pr, ep) => new { pr, ep })
                .GroupBy(x => new { x.pr.ProjectId, x.ep.RoleInProject })
                .Select(g => new RoleAverageScoreDto
                {
                    ProjectId = g.Key.ProjectId,
                    Role = g.Key.RoleInProject,
                    AverageScore = g.Average(x => x.pr.Score) ?? 0
                })
                .ToList();

            return Ok(data);
        }

    }
}

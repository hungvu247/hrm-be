using human_resource_management.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestTypeController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;
        public RequestTypeController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        // GET: api/RequestTypes
        [HttpGet]
        [Produces("application/json")]

        public IActionResult GetAllRequestTypes()
        {
            var requestTypes = _context.RequestTypes
                .Select(rt => new
                {
                    rt.RequestTypeId,
                    rt.RequestTypeName
                })
                .ToList();

            return Ok(requestTypes);
        }
    }
}

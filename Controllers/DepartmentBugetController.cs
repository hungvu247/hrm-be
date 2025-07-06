using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentBugetController : ControllerBase
    {
        private readonly Service.DepartmentService _departmentService;
        public DepartmentBugetController(Service.DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
   
    }
}

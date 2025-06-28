using human_resource_management.Model;
using human_resource_management.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly JwtService _accountService;
        public AccountController(JwtService accountService)
        {
            _accountService = accountService;
        }
        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel loginRequestModel)
        {
            var result = await _accountService.Authenticate(loginRequestModel);
            if (result == null)
            {
                return Unauthorized();
            }
            return result;
        }
    }
}

using human_resource_management.Dto;
using human_resource_management.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {

        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost("request")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> CreateRequest([FromBody] PromotionRequestDto dto)
        {
            var result = await _promotionService.CreatePromotionRequest(dto);
            return Ok(result);
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Approve(int id, [FromQuery] int approvedBy)
        {
            var result = await _promotionService.ApproveRequest(id, approvedBy);
            return Ok(result);
        }

        [HttpGet("eligible")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetEligibleCandidates()
        {
            var result = await _promotionService.GetEligibleCandidates();
            return Ok(result);
        }
    }
}

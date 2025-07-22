using human_resource_management.Dto;

namespace human_resource_management.Service
{
    public interface IPromotionService
    {
        Task<object> CreatePromotionRequest(PromotionRequestDto dto);
        Task<object> ApproveRequest(int requestId, int approvedBy);
        Task<List<EligibleCandidateDto>> GetEligibleCandidates();
    }
}

    namespace human_resource_management.Dto
{
    public class PromotionRequestDto
    {
        public int EmployeeId { get; set; }
        public int CurrentPositionId { get; set; }
        public int SuggestedPositionId { get; set; }
        public string Reason { get; set; }
        public int RequestedBy { get; set; }
    }
}

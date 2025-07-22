namespace human_resource_management.Dto
{
    public class EligibleCandidateDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public int CurrentPositionId { get; set; }
        public double AverageKpi { get; set; }
        public int ReviewCount { get; set; }
        public DateTime FirstReview { get; set; }
        public DateTime LatestReview { get; set; }
    }
}

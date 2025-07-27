namespace human_resource_management.Dto
{
    public class EmployeeAverageScoreDto
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public double AverageScore { get; set; }
        public int TotalReviews { get; set; }
        public string Username { get; set; }
        public string ProjectName { get; set; }
    }
}

namespace human_resource_management.Dto
{
    public class PerformanceReviewDto
    {
 
    }
    public class PerformanceReviewUpdateDto
    {
        public DateTime? ReviewDate { get; set; }
        public int? Score { get; set; }
        public string? Comments { get; set; }
    }
    public class PerformanceReviewCreateDto
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int? Score { get; set; }
        public string? Comments { get; set; }
    }
    
    public class PerformanceReviewReadDto
    {
        public int ReviewId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int? Score { get; set; }
        public string? Comments { get; set; }
    }


}

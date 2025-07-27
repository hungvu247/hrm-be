namespace human_resource_management.Dto
{
    public class ProjectReviewStatisticDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TotalReviews { get; set; }
        public double AverageScore { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public List<ReviewDetailDto> Reviews { get; set; }
    }

}

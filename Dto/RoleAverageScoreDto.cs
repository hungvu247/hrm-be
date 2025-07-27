namespace human_resource_management.Dto
{
    public class RoleAverageScoreDto
    {
        public int ProjectId { get; set; }
        public string? Role { get; set; }
        public double AverageScore { get; set; }
    }
}

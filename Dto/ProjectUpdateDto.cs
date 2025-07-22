namespace human_resource_management.Dto
{
    public class ProjectUpdateDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

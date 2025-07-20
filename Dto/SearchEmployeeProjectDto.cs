namespace human_resource_management.Dto
{
    public class SearchEmployeeProjectDto
    {
        public string? Keyword { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

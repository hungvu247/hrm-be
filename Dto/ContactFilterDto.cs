namespace human_resource_management.Dto
{
    public class ContactFilterDto
    {
        public string? Keyword { get; set; }
        public int? EmployeeId { get; set; }
        public string? Type { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

namespace human_resource_management.Dto
{
    public class SearchEmployeeDto
    {
        public string? Keyword { get; set; }

        public int? PositionId { get; set; }

        public int? DepartmentId { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        public DateOnly? HireDateMin { get; set; }

        public DateOnly? HireDateMax { get; set; }
    }
}

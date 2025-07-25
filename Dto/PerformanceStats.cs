namespace human_resource_management.Dto
{
    public class PerformanceStats
    {
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public int? Score { get; set; }
        public DateOnly? ReviewDate { get; set; }
    }
}

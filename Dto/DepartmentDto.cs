namespace human_resource_management.Dto
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string? Description { get; set; }
        public List<EmployeeDto> Employees { get; set; } = new();
    }
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Position { get; set; }
    }
}

namespace human_resource_management.Dto
{
    public class EmployeeDto2
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateOnly? HireDate { get; set; }

        public decimal? Salary { get; set; }

        public string? Status { get; set; }

        public int? PositionId { get; set; }

        public int? DepartmentId { get; set; }

        public string? Email { get; set; }

        public int? RoleId { get; set; }

        public string? DepartmentName { get; set; }

        public string? PositionName { get; set; }

        public string? RoleName { get; set; }
    }
}

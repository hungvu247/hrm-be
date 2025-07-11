namespace human_resource_management.Dto
{
    public class UpdateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly? HireDate { get; set; }
        public decimal? Salary { get; set; }
        public string? Status { get; set; }
        public int? PositionId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } // sẽ mã hóa nếu không null
        public string? Email { get; set; }
        public int? RoleId { get; set; }
    }
}

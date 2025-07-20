namespace human_resource_management.Dto
{
    public class AddProjectEmployeeDto
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string? RoleInProject { get; set; }
    }
}

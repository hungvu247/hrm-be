using human_resource_management.Model;

namespace human_resource_management.Dto
{
    public class PositionTreeDto
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }

        public List<EmployeeDto> Employees { get; set; } = new();
    }
    public class PositionDto
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; } = null!;
    }
}


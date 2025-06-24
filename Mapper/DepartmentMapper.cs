using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public class DepartmentMapper
    {
        public static DepartmentDto ToDto(Department department)
        {
            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description,
                Employees = department.Employees?.Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Position = e.Position?.PositionName
                }).ToList() ?? new List<EmployeeDto>()
            };
        }

        public static Department ToEntity(DepartmentDto dto)
        {
            return new Department
            {
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                Description = dto.Description,

            };
        }
    }
}

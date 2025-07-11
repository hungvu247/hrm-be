using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public static class EmployeeMapper
    {
        public static EmployeeDto2 ToDto(Employee employee)
        {
            return new EmployeeDto2
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                HireDate = employee.HireDate,
                Salary = employee.Salary,
                Status = employee.Status,
                PositionId = employee.PositionId,
                DepartmentId = employee.DepartmentId,
                Email = employee.Email,
                RoleId = employee.RoleId,
                DepartmentName = employee.Department?.DepartmentName,
                PositionName = employee.Position?.PositionName,
                RoleName = employee.Role?.RoleName
            };
        }


        public static Employee ToEntity(CreateEmployeeDto dto)
        {
            return new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                HireDate = dto.HireDate,
                Salary = dto.Salary,
                Status = dto.Status,
                PositionId = dto.PositionId,
                DepartmentId = dto.DepartmentId,
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                RoleId = dto.RoleId
            };
        }

    }

}

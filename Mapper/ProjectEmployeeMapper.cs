using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public static class ProjectEmployeeMapper
    {
        public static ProjectEmployeeDto ToDto(EmployeeProject entity)
        {
            return new ProjectEmployeeDto
            {
                EmployeeID = entity.EmployeeId,
                UserName = entity.Employee?.Username ?? string.Empty,
                ProjectName = entity.Project?.ProjectName ?? string.Empty,
                RoleInProject = entity.RoleInProject
            };
        }

        public static EmployeeProject ToEntity(ProjectEmployeeDto dto)
        {
            return new EmployeeProject
            {
                // Nếu tạo mới từ DTO, bạn thường cần map ID thay vì toàn bộ object:
                // ProjectId, EmployeeId nên lấy từ nơi khác (ví dụ Controller).
                // Ở đây để ví dụ:
                RoleInProject = dto.RoleInProject,

                Employee = new Employee
                {
                    Username = dto.UserName
                },

                Project = new Project
                {
                    ProjectName = dto.ProjectName
                }
            };
        }

        public static AddProjectEmployeeDto ToDto2(EmployeeProject ep)
        {
            return new AddProjectEmployeeDto
            {
                EmployeeId = ep.EmployeeId,
                ProjectId = ep.ProjectId,
                RoleInProject = ep.RoleInProject
            };
        }

        public static EmployeeProject ToEntity2(AddProjectEmployeeDto dto)
        {
            return new EmployeeProject
            {
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId,
                RoleInProject = dto.RoleInProject
            };
        }
    }
}

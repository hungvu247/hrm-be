using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public static class RoleMapper
    {
        public static RoleDto ToDto(Role role)
        {
            return new RoleDto
            {
               RoleId = role.RoleId,
               RoleName = role.RoleName
            };
        }
    }
}

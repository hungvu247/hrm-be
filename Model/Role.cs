using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}

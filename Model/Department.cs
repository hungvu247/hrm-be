using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<DepartmentBudget> DepartmentBudgets { get; set; } = new List<DepartmentBudget>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

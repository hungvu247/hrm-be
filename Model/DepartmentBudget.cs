using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class DepartmentBudget
{
    public int BudgetId { get; set; }

    public int DepartmentId { get; set; }

    public int Year { get; set; }

    public decimal? AllocatedBudget { get; set; }

    public decimal? UsedBudget { get; set; }

    public virtual Department Department { get; set; } = null!;
}

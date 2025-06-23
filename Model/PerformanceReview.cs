using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class PerformanceReview
{
    public int ReviewId { get; set; }

    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public int? Score { get; set; }

    public string? Comments { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

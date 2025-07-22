using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class EligibleForPromotion
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public int? PositionId { get; set; }

    public int? AvgKpi { get; set; }

    public int? ReviewCount { get; set; }

    public DateOnly? FirstReview { get; set; }

    public DateOnly? LatestReview { get; set; }
}

using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class PromotionHistory
{
    public int PromotionId { get; set; }

    public int EmployeeId { get; set; }

    public string? OldPosition { get; set; }

    public string? NewPosition { get; set; }

    public DateOnly? PromotionDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}

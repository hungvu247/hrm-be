using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class PromotionRequest
{
    public int RequestId { get; set; }

    public int EmployeeId { get; set; }

    public int CurrentPositionId { get; set; }

    public int SuggestedPositionId { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public int RequestedBy { get; set; }

    public int? ApprovedBy { get; set; }

    public DateOnly? RequestDate { get; set; }

    public DateOnly? ApproveDate { get; set; }

    public int? AssignedTo { get; set; }

    public virtual Employee? ApprovedByNavigation { get; set; }

    public virtual Employee? AssignedToNavigation { get; set; }

    public virtual Position CurrentPosition { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee RequestedByNavigation { get; set; } = null!;

    public virtual Position SuggestedPosition { get; set; } = null!;
}

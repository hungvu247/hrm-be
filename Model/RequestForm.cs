using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class RequestForm
{
    public int RequestId { get; set; }

    public int EmployeeId { get; set; }

    public int RequestTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewComment { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual RequestType RequestType { get; set; } = null!;

    public virtual Employee? ReviewedByNavigation { get; set; }
}

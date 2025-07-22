using System;
using System.Collections.Generic;

namespace human_resource_management.Models;

public partial class EmployeeContact
{
    public int ContactId { get; set; }

    public int EmployeeId { get; set; }

    public string? ContactType { get; set; }

    public string? ContactValue { get; set; }

    public bool? IsPrimary { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}

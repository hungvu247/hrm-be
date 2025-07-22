﻿using System;
using System.Collections.Generic;

namespace human_resource_management.Models;

public partial class EmployeeProject
{
    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public string? RoleInProject { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

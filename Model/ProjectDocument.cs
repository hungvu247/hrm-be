using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class ProjectDocument
{
    public int DocumentId { get; set; }

    public int ProjectId { get; set; }

    public string? DocumentName { get; set; }

    public string? FilePath { get; set; }

    public DateOnly? UploadDate { get; set; }

    public virtual Project Project { get; set; } = null!;
}

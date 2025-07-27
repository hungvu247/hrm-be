using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class RequestType
{
    public int RequestTypeId { get; set; }

    public string RequestTypeName { get; set; } = null!;

    public virtual ICollection<RequestForm> RequestForms { get; set; } = new List<RequestForm>();
}

using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public string JwtId { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public bool IsUsed { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime AddedDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public virtual Employee? Employee { get; set; }
}

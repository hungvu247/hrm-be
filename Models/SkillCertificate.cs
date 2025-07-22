using System;
using System.Collections.Generic;

namespace human_resource_management.Models;

public partial class SkillCertificate
{
    public int CertificateId { get; set; }

    public int SkillId { get; set; }

    public string? CertificateName { get; set; }

    public string? IssuedBy { get; set; }

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }
}

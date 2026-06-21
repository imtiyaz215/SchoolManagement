using Shared.Domain;

namespace Operations.Domain.Entities;

public class CertificateTemplate : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
}

public class StudentCertificate : TenantEntity
{
    public Guid StudentId { get; set; }
    public Guid CertificateTemplateId { get; set; }
    public CertificateTemplate? CertificateTemplate { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public string? Remarks { get; set; }
    public bool IsFeeCertificate { get; set; }
}

public class GatePass : TenantEntity
{
    public string GatePassNumber { get; set; } = string.Empty;
    public string Type { get; set; } = "Student";
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
}

public class StudentGatePass
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid GatePassId { get; set; }
    public GatePass? GatePass { get; set; }
    public Guid StudentId { get; set; }
    public string? AccompaniedBy { get; set; }
    public string? Relation { get; set; }
}

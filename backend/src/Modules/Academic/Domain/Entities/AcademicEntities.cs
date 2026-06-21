using Academic.Domain.Enums;
using Shared.Domain;

namespace Academic.Domain.Entities;

public class StudentEnrollment : TenantEntity
{
    public Guid StudentId { get; set; }
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SectionId { get; set; }
    public string? RollNumber { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    public ICollection<EnrollmentHistory> History { get; set; } = new List<EnrollmentHistory>();
}

public class EnrollmentHistory : TenantEntity
{
    public Guid StudentEnrollmentId { get; set; }
    public StudentEnrollment? StudentEnrollment { get; set; }
    public EnrollmentActionType ActionType { get; set; }
    public Guid? OldClassId { get; set; }
    public Guid? OldSectionId { get; set; }
    public Guid? NewClassId { get; set; }
    public Guid? NewSectionId { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
}

public class Teacher : TenantEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? SignaturePath { get; set; }
}

public class ClassInchargeAssignment : TenantEntity
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SectionId { get; set; }
    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}

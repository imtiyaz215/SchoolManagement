using Academic.Domain.Enums;

namespace Academic.Contracts;

public record CreateEnrollmentRequest(
    Guid StudentId,
    Guid AcademicSessionId,
    Guid ClassId,
    Guid SectionId,
    string? RollNumber);

public record PromoteStudentsRequest(
    Guid FromSessionId,
    Guid ToSessionId,
    Guid FromClassId,
    Guid ToClassId,
    IReadOnlyList<Guid> StudentIds);

public record SectionTransferRequest(
    Guid StudentEnrollmentId,
    Guid NewSectionId,
    string? Reason);

public record CreateTeacherRequest(
    string Code,
    string Name,
    string? Mobile,
    string? Email);

public record AssignClassInchargeRequest(
    Guid AcademicSessionId,
    Guid ClassId,
    Guid SectionId,
    Guid TeacherId);

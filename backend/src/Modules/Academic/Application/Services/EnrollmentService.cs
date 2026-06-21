using Academic.Contracts;
using Academic.Domain.Entities;
using Academic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Academic.Application.Services;

public interface IEnrollmentService
{
    Task<Result<StudentEnrollment>> CreateAsync(CreateEnrollmentRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<StudentEnrollment>> ListBySessionAsync(Guid sessionId, CancellationToken ct = default);
    Task<IReadOnlyList<StudentEnrollment>> ListByStudentAsync(Guid studentId, CancellationToken ct = default);
    Task<Result<int>> PromoteAsync(PromoteStudentsRequest req, CancellationToken ct = default);
    Task<Result<StudentEnrollment>> TransferSectionAsync(SectionTransferRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<EnrollmentHistory>> GetHistoryAsync(Guid enrollmentId, CancellationToken ct = default);
}

public class EnrollmentService : IEnrollmentService
{
    private readonly DbContext _db;
    public EnrollmentService(DbContext db) => _db = db;

    public async Task<Result<StudentEnrollment>> CreateAsync(CreateEnrollmentRequest req, CancellationToken ct = default)
    {
        var existing = await _db.Set<StudentEnrollment>().AnyAsync(e =>
            e.StudentId == req.StudentId &&
            e.AcademicSessionId == req.AcademicSessionId &&
            e.Status == EnrollmentStatus.Active, ct);

        if (existing)
            return Result<StudentEnrollment>.Failure("Student already has an active enrollment for this session.", "duplicate_enrollment");

        if (!string.IsNullOrEmpty(req.RollNumber))
        {
            var rollTaken = await _db.Set<StudentEnrollment>().AnyAsync(e =>
                e.AcademicSessionId == req.AcademicSessionId &&
                e.ClassId == req.ClassId &&
                e.SectionId == req.SectionId &&
                e.RollNumber == req.RollNumber, ct);
            if (rollTaken)
                return Result<StudentEnrollment>.Failure("Roll number already taken in this class/section.", "roll_conflict");
        }

        var enrollment = new StudentEnrollment
        {
            StudentId = req.StudentId,
            AcademicSessionId = req.AcademicSessionId,
            ClassId = req.ClassId,
            SectionId = req.SectionId,
            RollNumber = req.RollNumber,
            Status = EnrollmentStatus.Active
        };
        _db.Set<StudentEnrollment>().Add(enrollment);
        await _db.SaveChangesAsync(ct);

        _db.Set<EnrollmentHistory>().Add(new EnrollmentHistory
        {
            StudentEnrollmentId = enrollment.Id,
            ActionType = EnrollmentActionType.Admitted,
            NewClassId = req.ClassId,
            NewSectionId = req.SectionId,
            Reason = "Admission"
        });
        await _db.SaveChangesAsync(ct);

        return Result<StudentEnrollment>.Success(enrollment);
    }

    public async Task<IReadOnlyList<StudentEnrollment>> ListBySessionAsync(Guid sessionId, CancellationToken ct = default) =>
        await _db.Set<StudentEnrollment>()
            .Where(e => e.AcademicSessionId == sessionId && e.Status == EnrollmentStatus.Active)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<StudentEnrollment>> ListByStudentAsync(Guid studentId, CancellationToken ct = default) =>
        await _db.Set<StudentEnrollment>()
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(ct);

    public async Task<Result<int>> PromoteAsync(PromoteStudentsRequest req, CancellationToken ct = default)
    {
        int count = 0;
        foreach (var sid in req.StudentIds)
        {
            var current = await _db.Set<StudentEnrollment>().FirstOrDefaultAsync(e =>
                e.StudentId == sid && e.AcademicSessionId == req.FromSessionId && e.Status == EnrollmentStatus.Active, ct);
            if (current is null) continue;

            current.Status = EnrollmentStatus.Inactive;
            _db.Set<EnrollmentHistory>().Add(new EnrollmentHistory
            {
                StudentEnrollmentId = current.Id,
                ActionType = EnrollmentActionType.Promoted,
                OldClassId = current.ClassId,
                OldSectionId = current.SectionId,
                NewClassId = req.ToClassId
            });

            var newEnrollment = new StudentEnrollment
            {
                StudentId = sid,
                AcademicSessionId = req.ToSessionId,
                ClassId = req.ToClassId,
                SectionId = current.SectionId,
                RollNumber = current.RollNumber,
                Status = EnrollmentStatus.Active
            };
            _db.Set<StudentEnrollment>().Add(newEnrollment);
            count++;
        }
        await _db.SaveChangesAsync(ct);
        return Result<int>.Success(count);
    }

    public async Task<Result<StudentEnrollment>> TransferSectionAsync(SectionTransferRequest req, CancellationToken ct = default)
    {
        var enrollment = await _db.Set<StudentEnrollment>().FirstOrDefaultAsync(e => e.Id == req.StudentEnrollmentId, ct);
        if (enrollment is null) return Result<StudentEnrollment>.Failure("Enrollment not found.", "not_found");
        if (enrollment.Status != EnrollmentStatus.Active)
            return Result<StudentEnrollment>.Failure("Only active enrollments can be transferred.", "invalid_status");

        var oldSection = enrollment.SectionId;
        enrollment.SectionId = req.NewSectionId;
        _db.Set<EnrollmentHistory>().Add(new EnrollmentHistory
        {
            StudentEnrollmentId = enrollment.Id,
            ActionType = EnrollmentActionType.SectionChanged,
            OldSectionId = oldSection,
            NewSectionId = req.NewSectionId,
            Reason = req.Reason
        });
        await _db.SaveChangesAsync(ct);
        return Result<StudentEnrollment>.Success(enrollment);
    }

    public async Task<IReadOnlyList<EnrollmentHistory>> GetHistoryAsync(Guid enrollmentId, CancellationToken ct = default) =>
        await _db.Set<EnrollmentHistory>()
            .Where(h => h.StudentEnrollmentId == enrollmentId)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync(ct);
}

using Academic.Contracts;
using Academic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Academic.Application.Services;

public interface ITeacherService
{
    Task<Result<Teacher>> CreateAsync(CreateTeacherRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<Teacher>> ListAsync(CancellationToken ct = default);
    Task<Result<ClassInchargeAssignment>> AssignInchargeAsync(AssignClassInchargeRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ClassInchargeAssignment>> GetInchargesAsync(Guid sessionId, CancellationToken ct = default);
}

public class TeacherService : ITeacherService
{
    private readonly DbContext _db;
    public TeacherService(DbContext db) => _db = db;

    public async Task<Result<Teacher>> CreateAsync(CreateTeacherRequest req, CancellationToken ct = default)
    {
        var teacher = new Teacher { Code = req.Code, Name = req.Name, Mobile = req.Mobile, Email = req.Email };
        _db.Set<Teacher>().Add(teacher);
        await _db.SaveChangesAsync(ct);
        return Result<Teacher>.Success(teacher);
    }

    public async Task<IReadOnlyList<Teacher>> ListAsync(CancellationToken ct = default) =>
        await _db.Set<Teacher>().OrderBy(t => t.Name).ToListAsync(ct);

    public async Task<Result<ClassInchargeAssignment>> AssignInchargeAsync(AssignClassInchargeRequest req, CancellationToken ct = default)
    {
        var existing = await _db.Set<ClassInchargeAssignment>().FirstOrDefaultAsync(a =>
            a.AcademicSessionId == req.AcademicSessionId &&
            a.ClassId == req.ClassId &&
            a.SectionId == req.SectionId, ct);

        if (existing is not null)
        {
            existing.TeacherId = req.TeacherId;
        }
        else
        {
            existing = new ClassInchargeAssignment
            {
                AcademicSessionId = req.AcademicSessionId,
                ClassId = req.ClassId,
                SectionId = req.SectionId,
                TeacherId = req.TeacherId
            };
            _db.Set<ClassInchargeAssignment>().Add(existing);
        }
        await _db.SaveChangesAsync(ct);
        return Result<ClassInchargeAssignment>.Success(existing);
    }

    public async Task<IReadOnlyList<ClassInchargeAssignment>> GetInchargesAsync(Guid sessionId, CancellationToken ct = default) =>
        await _db.Set<ClassInchargeAssignment>()
            .Include(a => a.Teacher)
            .Where(a => a.AcademicSessionId == sessionId)
            .ToListAsync(ct);
}

using Microsoft.EntityFrameworkCore;
using Reference.Domain.Entities;
using Shared.Exceptions;
using Shared.Results;

namespace Reference.Application.Services;

public record CreateSessionRequest(string Name, DateTime StartDate, DateTime EndDate);
public record CreateClassGroupRequest(string Name, string? AdmissionPrefix, int SequenceNo);
public record CreateClassRequest(Guid ClassGroupId, string Name, int SequenceNo);
public record CreateSectionRequest(Guid ClassId, string Name, int Capacity);

public interface IAcademicService
{
    Task<Result<AcademicSession>> CreateSessionAsync(CreateSessionRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<AcademicSession>> ListSessionsAsync(CancellationToken ct = default);
    Task<Result<ClassGroup>> CreateClassGroupAsync(CreateClassGroupRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ClassGroup>> ListClassGroupsAsync(CancellationToken ct = default);
    Task<Result<ClassEntity>> CreateClassAsync(CreateClassRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ClassEntity>> ListClassesAsync(CancellationToken ct = default);
    Task<Result<Section>> CreateSectionAsync(CreateSectionRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<Section>> ListSectionsAsync(Guid classId, CancellationToken ct = default);
}

public class AcademicService : IAcademicService
{
    private readonly DbContext _db;
    public AcademicService(DbContext db) => _db = db;

    public async Task<Result<AcademicSession>> CreateSessionAsync(CreateSessionRequest req, CancellationToken ct = default)
    {
        var entity = new AcademicSession { Name = req.Name, StartDate = req.StartDate, EndDate = req.EndDate };
        if (req.EndDate <= req.StartDate)
            return Result<AcademicSession>.Failure("End date must be after start date.", "validation");

        var existingActive = await _db.Set<AcademicSession>().Where(s => s.IsActive).ToListAsync(ct);
        foreach (var s in existingActive) s.IsActive = false;

        entity.IsActive = true;
        _db.Set<AcademicSession>().Add(entity);
        await _db.SaveChangesAsync(ct);
        return Result<AcademicSession>.Success(entity);
    }

    public Task<IReadOnlyList<AcademicSession>> ListSessionsAsync(CancellationToken ct = default) =>
        _db.Set<AcademicSession>().OrderByDescending(s => s.StartDate).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<AcademicSession>)t.Result, ct);

    public async Task<Result<ClassGroup>> CreateClassGroupAsync(CreateClassGroupRequest req, CancellationToken ct = default)
    {
        var entity = new ClassGroup { Name = req.Name, AdmissionPrefix = req.AdmissionPrefix, SequenceNo = req.SequenceNo };
        _db.Set<ClassGroup>().Add(entity);
        await _db.SaveChangesAsync(ct);
        return Result<ClassGroup>.Success(entity);
    }

    public Task<IReadOnlyList<ClassGroup>> ListClassGroupsAsync(CancellationToken ct = default) =>
        _db.Set<ClassGroup>().OrderBy(g => g.SequenceNo).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<ClassGroup>)t.Result, ct);

    public async Task<Result<ClassEntity>> CreateClassAsync(CreateClassRequest req, CancellationToken ct = default)
    {
        var entity = new ClassEntity { ClassGroupId = req.ClassGroupId, Name = req.Name, SequenceNo = req.SequenceNo };
        _db.Set<ClassEntity>().Add(entity);
        await _db.SaveChangesAsync(ct);
        return Result<ClassEntity>.Success(entity);
    }

    public Task<IReadOnlyList<ClassEntity>> ListClassesAsync(CancellationToken ct = default) =>
        _db.Set<ClassEntity>().Include(c => c.ClassGroup).OrderBy(c => c.SequenceNo).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<ClassEntity>)t.Result, ct);

    public async Task<Result<Section>> CreateSectionAsync(CreateSectionRequest req, CancellationToken ct = default)
    {
        var entity = new Section { ClassId = req.ClassId, Name = req.Name, Capacity = req.Capacity };
        _db.Set<Section>().Add(entity);
        await _db.SaveChangesAsync(ct);
        return Result<Section>.Success(entity);
    }

    public async Task<IReadOnlyList<Section>> ListSectionsAsync(Guid classId, CancellationToken ct = default) =>
        await _db.Set<Section>().Where(s => s.ClassId == classId).OrderBy(s => s.Name).ToListAsync(ct);
}

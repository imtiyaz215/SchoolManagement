using Microsoft.EntityFrameworkCore;
using Shared.Results;
using Student.Contracts;
using Student.Domain.Entities;
using StudentEntity = Student.Domain.Entities.StudentEntity;
using Student.Domain.Enums;

namespace Student.Application.Services;

public interface IStudentService
{
    Task<Result<StudentEntity>> CreateAsync(CreateStudentRequest req, CancellationToken ct = default);
    Task<StudentEntity?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<StudentListItem>> ListAsync(string? search, int skip, int take, CancellationToken ct = default);
    Task<int> CountAsync(string? search, CancellationToken ct = default);
    Task<Result<StudentEntity>> UpdateStatusAsync(Guid id, StudentStatus status, DateOnly effectiveDate, string? reason, CancellationToken ct = default);
}

public class StudentService : IStudentService
{
    private readonly DbContext _db;
    public StudentService(DbContext db) => _db = db;

    public async Task<Result<StudentEntity>> CreateAsync(CreateStudentRequest req, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.AdmissionNumber))
            return Result<StudentEntity>.Failure("Admission number is required.", "validation");

        var student = new StudentEntity
        {
            AdmissionNumber = req.AdmissionNumber,
            FirstName = req.FirstName,
            MiddleName = req.MiddleName,
            LastName = req.LastName,
            Gender = req.Gender,
            DOB = req.DOB,
            ReligionId = req.ReligionId,
            CasteId = req.CasteId,
            ScholarTypeId = req.ScholarTypeId,
            Phone = req.Phone,
            Email = req.Email,
            Remarks = req.Remarks,
            Status = StudentStatus.Active
        };

        _db.Set<StudentEntity>().Add(student);

        if (req.Parents is not null)
        {
            foreach (var p in req.Parents)
            {
                var parent = new Parent
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Phone = p.Phone,
                    Email = p.Email
                };
                _db.Set<Parent>().Add(parent);
                await _db.SaveChangesAsync(ct);
                _db.Set<StudentParent>().Add(new StudentParent
                {
                    StudentId = student.Id,
                    ParentId = parent.Id,
                    Relationship = p.Relationship
                });
            }
        }

        if (req.Addresses is not null)
        {
            foreach (var a in req.Addresses)
            {
                _db.Set<Address>().Add(new Address
                {
                    StudentId = student.Id,
                    AddressType = a.AddressType,
                    Line1 = a.Line1,
                    CityId = a.CityId
                });
            }
        }

        _db.Set<StudentStatusHistory>().Add(new StudentStatusHistory
        {
            StudentId = student.Id,
            Status = StudentStatus.Active,
            EffectiveDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Reason = "Admission"
        });

        await _db.SaveChangesAsync(ct);
        return Result<StudentEntity>.Success(student);
    }

    public async Task<StudentEntity?> GetAsync(Guid id, CancellationToken ct = default) =>
        await _db.Set<StudentEntity>()
            .Include(s => s.Parents).ThenInclude(sp => sp.Parent)
            .Include(s => s.Addresses)
            .Include(s => s.Documents)
            .Include(s => s.StatusHistory)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<StudentListItem>> ListAsync(string? search, int skip, int take, CancellationToken ct = default)
    {
        var query = _db.Set<StudentEntity>().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(x =>
                x.AdmissionNumber.ToLower().Contains(s) ||
                x.FirstName.ToLower().Contains(s) ||
                x.LastName.ToLower().Contains(s));
        }
        return await query
            .OrderBy(x => x.AdmissionNumber)
            .Skip(skip).Take(take)
            .Select(x => new StudentListItem(x.Id, x.AdmissionNumber,
                x.FirstName + " " + x.LastName, x.Gender, x.DOB, x.Status.ToString()))
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search, CancellationToken ct = default)
    {
        var query = _db.Set<StudentEntity>().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(x =>
                x.AdmissionNumber.ToLower().Contains(s) ||
                x.FirstName.ToLower().Contains(s) ||
                x.LastName.ToLower().Contains(s));
        }
        return await query.CountAsync(ct);
    }

    public async Task<Result<StudentEntity>> UpdateStatusAsync(Guid id, StudentStatus status, DateOnly effectiveDate, string? reason, CancellationToken ct = default)
    {
        var student = await _db.Set<StudentEntity>().FirstOrDefaultAsync(s => s.Id == id, ct);
        if (student is null) return Result<StudentEntity>.Failure("Student not found.", "not_found");

        student.Status = status;
        _db.Set<StudentStatusHistory>().Add(new StudentStatusHistory
        {
            StudentId = student.Id,
            Status = status,
            EffectiveDate = effectiveDate,
            Reason = reason
        });
        await _db.SaveChangesAsync(ct);
        return Result<StudentEntity>.Success(student);
    }
}

using Identity.Contracts;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Identity.Application.Services;

public record CreateSchoolRequest(string Name, string? Code, string? Email, string? Phone, string? Address);

public interface ISchoolService
{
    Task<Result<School>> CreateAsync(CreateSchoolRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<School>> ListAsync(CancellationToken ct = default);
}

public class SchoolService : ISchoolService
{
    private readonly DbContext _db;
    public SchoolService(DbContext db) => _db = db;

    public async Task<Result<School>> CreateAsync(CreateSchoolRequest request, CancellationToken ct = default)
    {
        var school = new School
        {
            Name = request.Name,
            Code = request.Code,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        };
        _db.Set<School>().Add(school);
        await _db.SaveChangesAsync(ct);
        return Result<School>.Success(school);
    }

    public async Task<IReadOnlyList<School>> ListAsync(CancellationToken ct = default) =>
        await _db.Set<School>().OrderBy(s => s.Name).ToListAsync(ct);
}

using Microsoft.EntityFrameworkCore;
using Reference.Domain.Entities;
using Shared.Results;

namespace Reference.Application.Services;

public record CreateLookupRequest(string Name, string? Code = null, string? PinCode = null, string? StdCode = null);

public interface ILookupService
{
    Task<IReadOnlyList<Religion>> ListReligionsAsync(CancellationToken ct = default);
    Task<Religion> CreateReligionAsync(string name, CancellationToken ct = default);
    Task<IReadOnlyList<Caste>> ListCastesAsync(CancellationToken ct = default);
    Task<Caste> CreateCasteAsync(string name, bool reserved, CancellationToken ct = default);
    Task<IReadOnlyList<House>> ListHousesAsync(CancellationToken ct = default);
    Task<House> CreateHouseAsync(string name, string? color, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarType>> ListScholarTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Qualification>> ListQualificationsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Occupation>> ListOccupationsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ParentDesignation>> ListDesignationsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<State>> ListStatesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<District>> ListDistrictsAsync(Guid stateId, CancellationToken ct = default);
    Task<IReadOnlyList<City>> ListCitiesAsync(Guid districtId, CancellationToken ct = default);
}

public class LookupService : ILookupService
{
    private readonly DbContext _db;
    public LookupService(DbContext db) => _db = db;

    public async Task<IReadOnlyList<Religion>> ListReligionsAsync(CancellationToken ct = default) =>
        await _db.Set<Religion>().OrderBy(r => r.Name).ToListAsync(ct);

    public async Task<Religion> CreateReligionAsync(string name, CancellationToken ct = default)
    {
        var r = new Religion { Name = name };
        _db.Set<Religion>().Add(r);
        await _db.SaveChangesAsync(ct);
        return r;
    }

    public async Task<IReadOnlyList<Caste>> ListCastesAsync(CancellationToken ct = default) =>
        await _db.Set<Caste>().OrderBy(c => c.Name).ToListAsync(ct);

    public async Task<Caste> CreateCasteAsync(string name, bool reserved, CancellationToken ct = default)
    {
        var c = new Caste { Name = name, IsReservedCategory = reserved };
        _db.Set<Caste>().Add(c);
        await _db.SaveChangesAsync(ct);
        return c;
    }

    public async Task<IReadOnlyList<House>> ListHousesAsync(CancellationToken ct = default) =>
        await _db.Set<House>().OrderBy(h => h.Name).ToListAsync(ct);

    public async Task<House> CreateHouseAsync(string name, string? color, CancellationToken ct = default)
    {
        var h = new House { Name = name, Color = color };
        _db.Set<House>().Add(h);
        await _db.SaveChangesAsync(ct);
        return h;
    }

    public async Task<IReadOnlyList<ScholarType>> ListScholarTypesAsync(CancellationToken ct = default) =>
        await _db.Set<ScholarType>().OrderBy(s => s.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Qualification>> ListQualificationsAsync(CancellationToken ct = default) =>
        await _db.Set<Qualification>().OrderBy(q => q.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Occupation>> ListOccupationsAsync(CancellationToken ct = default) =>
        await _db.Set<Occupation>().OrderBy(o => o.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<ParentDesignation>> ListDesignationsAsync(CancellationToken ct = default) =>
        await _db.Set<ParentDesignation>().OrderBy(d => d.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<State>> ListStatesAsync(CancellationToken ct = default) =>
        await _db.Set<State>().OrderBy(s => s.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<District>> ListDistrictsAsync(Guid stateId, CancellationToken ct = default) =>
        await _db.Set<District>().Where(d => d.StateId == stateId).OrderBy(d => d.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<City>> ListCitiesAsync(Guid districtId, CancellationToken ct = default) =>
        await _db.Set<City>().Where(c => c.DistrictId == districtId).OrderBy(c => c.Name).ToListAsync(ct);
}

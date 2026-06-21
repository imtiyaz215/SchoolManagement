using Behaviour.Contracts;
using Behaviour.Domain.Entities;
using Behaviour.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Behaviour.Application.Services;

public interface IBehaviourService
{
    Task<Result<BehaviourTemplate>> CreateTemplateAsync(CreateBehaviourTemplateRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<BehaviourTemplate>> ListTemplatesAsync(CancellationToken ct = default);
    Task<Result<BehaviourSheet>> SubmitSheetAsync(SubmitBehaviourSheetRequest req, Guid parentId, CancellationToken ct = default);
    Task<BehaviourSheet?> GetSheetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<BehaviourSheet>> ListSheetsAsync(Guid studentId, int? month, int? year, CancellationToken ct = default);
    Task<Result<BehaviourSheet>> ReviewAsync(Guid sheetId, BehaviourSheetStatus newStatus, CancellationToken ct = default);
}

public class BehaviourService : IBehaviourService
{
    private readonly DbContext _db;
    public BehaviourService(DbContext db) => _db = db;

    public async Task<Result<BehaviourTemplate>> CreateTemplateAsync(CreateBehaviourTemplateRequest req, CancellationToken ct = default)
    {
        var template = new BehaviourTemplate { Name = req.Name };
        _db.Set<BehaviourTemplate>().Add(template);
        await _db.SaveChangesAsync(ct);

        foreach (var i in req.Items)
        {
            _db.Set<BehaviourItem>().Add(new BehaviourItem
            {
                TemplateId = template.Id,
                Name = i.Name,
                DisplayOrder = i.DisplayOrder,
                InputType = i.InputType
            });
        }
        await _db.SaveChangesAsync(ct);
        return Result<BehaviourTemplate>.Success(template);
    }

    public async Task<IReadOnlyList<BehaviourTemplate>> ListTemplatesAsync(CancellationToken ct = default) =>
        await _db.Set<BehaviourTemplate>()
            .Include(t => t.Items.OrderBy(i => i.DisplayOrder))
            .OrderBy(t => t.Name)
            .ToListAsync(ct);

    public async Task<Result<BehaviourSheet>> SubmitSheetAsync(SubmitBehaviourSheetRequest req, Guid parentId, CancellationToken ct = default)
    {
        var existing = await _db.Set<BehaviourSheet>().FirstOrDefaultAsync(s =>
            s.StudentId == req.StudentId && s.Month == req.Month && s.Year == req.Year, ct);

        var sheet = existing ?? new BehaviourSheet
        {
            StudentId = req.StudentId,
            ParentId = parentId,
            Month = req.Month,
            Year = req.Year,
            Status = BehaviourSheetStatus.Submitted
        };
        sheet.Status = BehaviourSheetStatus.Submitted;
        if (existing is null) _db.Set<BehaviourSheet>().Add(sheet);

        await _db.SaveChangesAsync(ct);

        foreach (var e in req.Entries)
        {
            _db.Set<BehaviourEntry>().Add(new BehaviourEntry
            {
                SheetId = sheet.Id,
                DayNo = e.DayNo,
                BehaviourItemId = e.BehaviourItemId,
                Value = e.Value
            });
        }
        await _db.SaveChangesAsync(ct);
        return Result<BehaviourSheet>.Success(sheet);
    }

    public async Task<BehaviourSheet?> GetSheetAsync(Guid id, CancellationToken ct = default) =>
        await _db.Set<BehaviourSheet>()
            .Include(s => s.Entries)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<BehaviourSheet>> ListSheetsAsync(Guid studentId, int? month, int? year, CancellationToken ct = default)
    {
        var q = _db.Set<BehaviourSheet>().Where(s => s.StudentId == studentId);
        if (month.HasValue) q = q.Where(s => s.Month == month.Value);
        if (year.HasValue) q = q.Where(s => s.Year == year.Value);
        return await q.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month).ToListAsync(ct);
    }

    public async Task<Result<BehaviourSheet>> ReviewAsync(Guid sheetId, BehaviourSheetStatus newStatus, CancellationToken ct = default)
    {
        var sheet = await _db.Set<BehaviourSheet>().FirstOrDefaultAsync(s => s.Id == sheetId, ct);
        if (sheet is null) return Result<BehaviourSheet>.Failure("Sheet not found.", "not_found");
        sheet.Status = newStatus;
        await _db.SaveChangesAsync(ct);
        return Result<BehaviourSheet>.Success(sheet);
    }
}

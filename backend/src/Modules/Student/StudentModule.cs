using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Module;
using Student.Application.Services;
using Student.Contracts;
using Student.Domain.Enums;

namespace Student;

public class StudentModule : IModule
{
    public string Name => "Student";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStudentService, StudentService>();
    }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/students").WithTags("Students").RequireAuthorization();

        group.MapGet("/", async (string? search, int? page, int? pageSize, IStudentService svc, CancellationToken ct) =>
        {
            var p = page ?? 1;
            var ps = Math.Clamp(pageSize ?? 20, 1, 100);
            var items = await svc.ListAsync(search, (p - 1) * ps, ps, ct);
            var total = await svc.CountAsync(search, ct);
            return Results.Ok(new { items, total, page = p, pageSize = ps });
        });

        group.MapGet("/{id:guid}", async (Guid id, IStudentService svc, CancellationToken ct) =>
        {
            var s = await svc.GetAsync(id, ct);
            return s is null ? Results.NotFound() : Results.Ok(s);
        });

        group.MapPost("/", async (CreateStudentRequest req, IStudentService svc, CancellationToken ct) =>
        {
            var result = await svc.CreateAsync(req, ct);
            return result.IsSuccess ? Results.Created($"/students/{result.Value!.Id}", result.Value) : Results.BadRequest(result.Error);
        });

        group.MapPatch("/{id:guid}/status", async (Guid id, UpdateStatusRequest req, IStudentService svc, CancellationToken ct) =>
        {
            var result = await svc.UpdateStatusAsync(id, req.Status, req.EffectiveDate, req.Reason, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });
    }
}

public record UpdateStatusRequest(StudentStatus Status, DateOnly EffectiveDate, string? Reason);

using Academic.Application.Services;
using Academic.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Module;

namespace Academic;

public class AcademicModule : IModule
{
    public string Name => "Academic";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<ITeacherService, TeacherService>();
    }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/academic/enrollments").WithTags("Enrollments").RequireAuthorization();

        group.MapGet("/", async (Guid? sessionId, Guid? studentId, IEnrollmentService svc, CancellationToken ct) =>
        {
            if (studentId.HasValue) return Results.Ok(await svc.ListByStudentAsync(studentId.Value, ct));
            if (sessionId.HasValue) return Results.Ok(await svc.ListBySessionAsync(sessionId.Value, ct));
            return Results.BadRequest("sessionId or studentId required");
        });

        group.MapPost("/", async (CreateEnrollmentRequest req, IEnrollmentService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/enrollments/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        group.MapPost("/promote", async (PromoteStudentsRequest req, IEnrollmentService svc, CancellationToken ct) =>
        {
            var r = await svc.PromoteAsync(req, ct);
            return r.IsSuccess ? Results.Ok(new { promoted = r.Value }) : Results.BadRequest(r.Error);
        });

        group.MapPost("/section-transfer", async (SectionTransferRequest req, IEnrollmentService svc, CancellationToken ct) =>
        {
            var r = await svc.TransferSectionAsync(req, ct);
            return r.IsSuccess ? Results.Ok(r.Value) : Results.BadRequest(r.Error);
        });

        group.MapGet("/{id:guid}/history", async (Guid id, IEnrollmentService svc, CancellationToken ct) =>
            Results.Ok(await svc.GetHistoryAsync(id, ct)));

        var teachers = endpoints.MapGroup("/api/v1/academic/teachers").WithTags("Teachers").RequireAuthorization();
        teachers.MapGet("/", async (ITeacherService svc, CancellationToken ct) => Results.Ok(await svc.ListAsync(ct)));
        teachers.MapPost("/", async (CreateTeacherRequest req, ITeacherService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/teachers/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        var incharge = endpoints.MapGroup("/api/v1/academic/incharge").WithTags("Incharge").RequireAuthorization();
        incharge.MapGet("/", async (Guid sessionId, ITeacherService svc, CancellationToken ct) =>
            Results.Ok(await svc.GetInchargesAsync(sessionId, ct)));
        incharge.MapPost("/", async (AssignClassInchargeRequest req, ITeacherService svc, CancellationToken ct) =>
        {
            var r = await svc.AssignInchargeAsync(req, ct);
            return r.IsSuccess ? Results.Ok(r.Value) : Results.BadRequest(r.Error);
        });
    }
}

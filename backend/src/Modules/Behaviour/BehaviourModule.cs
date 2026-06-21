using System.Security.Claims;
using Behaviour.Application.Services;
using Behaviour.Contracts;
using Behaviour.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Module;

namespace Behaviour;

public class BehaviourModule : IModule
{
    public string Name => "Behaviour";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBehaviourService, BehaviourService>();
    }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/behaviour").WithTags("Behaviour").RequireAuthorization();

        group.MapGet("/templates", async (IBehaviourService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListTemplatesAsync(ct)));

        group.MapPost("/templates", async (CreateBehaviourTemplateRequest req, IBehaviourService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateTemplateAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/behaviour/templates/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        group.MapPost("/sheets", async (SubmitBehaviourSheetRequest req, HttpContext ctx, IBehaviourService svc, CancellationToken ct) =>
        {
            var parentIdStr = ctx.User.FindFirstValue("sub") ?? ctx.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (parentIdStr is null || !Guid.TryParse(parentIdStr, out var parentId))
                return Results.Unauthorized();
            var r = await svc.SubmitSheetAsync(req, parentId, ct);
            return r.IsSuccess ? Results.Created($"/behaviour/sheets/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        group.MapGet("/sheets/{id:guid}", async (Guid id, IBehaviourService svc, CancellationToken ct) =>
        {
            var sheet = await svc.GetSheetAsync(id, ct);
            return sheet is null ? Results.NotFound() : Results.Ok(sheet);
        });

        group.MapGet("/students/{studentId:guid}/sheets", async (Guid studentId, int? month, int? year, IBehaviourService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListSheetsAsync(studentId, month, year, ct)));

        group.MapPost("/sheets/{id:guid}/review", async (Guid id, string status, IBehaviourService svc, CancellationToken ct) =>
        {
            if (!Enum.TryParse<BehaviourSheetStatus>(status, true, out var newStatus))
                return Results.BadRequest("Invalid status");
            var r = await svc.ReviewAsync(id, newStatus, ct);
            return r.IsSuccess ? Results.Ok(r.Value) : Results.NotFound(r.Error);
        });
    }
}

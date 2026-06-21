using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reference.Application.Services;
using Shared.Module;

namespace Reference;

public class ReferenceModule : IModule
{
    public string Name => "Reference";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAcademicService, AcademicService>();
        services.AddScoped<ILookupService, LookupService>();
    }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var academic = endpoints.MapGroup("/api/v1/academic").WithTags("Academic").RequireAuthorization();
        academic.MapGet("/sessions", async (IAcademicService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListSessionsAsync(ct)));
        academic.MapPost("/sessions", async (CreateSessionRequest req, IAcademicService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateSessionAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/academic/sessions/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        academic.MapGet("/class-groups", async (IAcademicService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListClassGroupsAsync(ct)));
        academic.MapPost("/class-groups", async (CreateClassGroupRequest req, IAcademicService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateClassGroupAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/academic/class-groups/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        academic.MapGet("/classes", async (IAcademicService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListClassesAsync(ct)));
        academic.MapPost("/classes", async (CreateClassRequest req, IAcademicService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateClassAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/academic/classes/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        academic.MapGet("/classes/{classId:guid}/sections", async (Guid classId, IAcademicService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListSectionsAsync(classId, ct)));
        academic.MapPost("/sections", async (CreateSectionRequest req, IAcademicService svc, CancellationToken ct) =>
        {
            var r = await svc.CreateSectionAsync(req, ct);
            return r.IsSuccess ? Results.Created($"/academic/sections/{r.Value!.Id}", r.Value) : Results.BadRequest(r.Error);
        });

        var ref_ = endpoints.MapGroup("/api/v1/reference").WithTags("Reference").RequireAuthorization();

        ref_.MapGet("/religions", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListReligionsAsync(ct)));
        ref_.MapPost("/religions", async (CreateLookupRequest req, ILookupService svc, CancellationToken ct) =>
            Results.Created($"/reference/religions/{Guid.NewGuid()}", await svc.CreateReligionAsync(req.Name, ct)));

        ref_.MapGet("/castes", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListCastesAsync(ct)));
        ref_.MapPost("/castes", async (CreateLookupRequest req, ILookupService svc, CancellationToken ct) =>
            Results.Created($"/reference/castes/{Guid.NewGuid()}", await svc.CreateCasteAsync(req.Name, false, ct)));

        ref_.MapGet("/houses", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListHousesAsync(ct)));
        ref_.MapPost("/houses", async (CreateLookupRequest req, ILookupService svc, CancellationToken ct) =>
            Results.Created($"/reference/houses/{Guid.NewGuid()}", await svc.CreateHouseAsync(req.Name, req.Code, ct)));

        ref_.MapGet("/scholar-types", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListScholarTypesAsync(ct)));
        ref_.MapGet("/qualifications", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListQualificationsAsync(ct)));
        ref_.MapGet("/occupations", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListOccupationsAsync(ct)));
        ref_.MapGet("/designations", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListDesignationsAsync(ct)));

        ref_.MapGet("/states", async (ILookupService svc, CancellationToken ct) => Results.Ok(await svc.ListStatesAsync(ct)));
        ref_.MapGet("/states/{stateId:guid}/districts", async (Guid stateId, ILookupService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListDistrictsAsync(stateId, ct)));
        ref_.MapGet("/districts/{districtId:guid}/cities", async (Guid districtId, ILookupService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListCitiesAsync(districtId, ct)));
    }
}

using System.Security.Claims;
using Identity.Application.Services;
using Identity.Contracts;
using Identity.Domain.Entities;
using Identity.Infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Module;
using Shared.Tenant;

namespace Identity;

public class IdentityModule : IModule
{
    public string Name => "Identity";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenSettings>(configuration.GetSection("Jwt"));
        services.AddSingleton<TokenSettings>(sp =>
        {
            var settings = new TokenSettings();
            configuration.GetSection("Jwt").Bind(settings);
            if (string.IsNullOrEmpty(settings.Secret))
                settings.Secret = "dev-secret-key-please-change-in-production-min-32-chars-long";
            return settings;
        });
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISchoolService, SchoolService>();
        services.AddScoped<IUserService, UserService>();
    }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginRequest req, IAuthService auth, CancellationToken ct) =>
        {
            var result = await auth.LoginAsync(req, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });

        group.MapPost("/refresh", async (RefreshTokenRequest req, IAuthService auth, CancellationToken ct) =>
        {
            var result = await auth.RefreshAsync(req.RefreshToken, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });

        group.MapGet("/me", async (HttpContext ctx, IUserService users, ITenantContext tenant, CancellationToken ct) =>
        {
            var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? ctx.User.FindFirstValue("sub");
            if (userId is null || !Guid.TryParse(userId, out var id))
                return Results.Unauthorized();

            var schoolId = tenant.SchoolId ?? Guid.Empty;
            var all = await users.ListAsync(schoolId, ct);
            var user = all.FirstOrDefault(u => u.Id == id);
            return user is null ? Results.NotFound() : Results.Ok(new { user.Id, user.Email, user.FullName, user.SchoolId });
        }).RequireAuthorization();

        var schools = endpoints.MapGroup("/api/v1/schools").WithTags("Schools");
        schools.MapGet("/", async (ISchoolService svc, CancellationToken ct) =>
            Results.Ok(await svc.ListAsync(ct)));
        schools.MapPost("/", async (CreateSchoolRequest req, ISchoolService svc, CancellationToken ct) =>
            Results.Created($"/schools/{Guid.NewGuid()}", await svc.CreateAsync(req, ct)));

        var users = endpoints.MapGroup("/api/v1/users").WithTags("Users").RequireAuthorization();
        users.MapGet("/", async (ITenantContext tenant, IUserService svc, CancellationToken ct) =>
        {
            if (tenant.SchoolId is null) return Results.BadRequest("School context missing.");
            return Results.Ok(await svc.ListAsync(tenant.SchoolId.Value, ct));
        });
        users.MapPost("/", async (CreateUserRequest req, IUserService svc, CancellationToken ct) =>
        {
            var result = await svc.CreateAsync(req, ct);
            return result.IsSuccess ? Results.Created($"/users/{result.Value!.Id}", result.Value) : Results.BadRequest(result.Error);
        });
    }
}

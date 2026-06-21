using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Tenant;

namespace Infrastructure.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx, ITenantContext tenant)
    {
        if (ctx.User?.Identity?.IsAuthenticated == true)
        {
            var schoolClaim = ctx.User.FindFirstValue("school_id");
            if (Guid.TryParse(schoolClaim, out var schoolId))
                tenant.SetSchool(schoolId);

            var userId = ctx.User.FindFirstValue("sub") ?? ctx.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out var uid))
                tenant.SetUser(uid);
        }
        await _next(ctx);
    }
}

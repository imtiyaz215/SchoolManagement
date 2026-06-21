using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Module;

namespace Operations;

public class OperationsModule : IModule
{
    public string Name => "Operations";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration) { }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/operations").WithTags("Operations").RequireAuthorization();
        group.MapGet("/certificates", () => Results.Ok(Array.Empty<object>()));
        group.MapGet("/gatepass", () => Results.Ok(Array.Empty<object>()));
    }
}

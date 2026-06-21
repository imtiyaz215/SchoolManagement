using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Module;

namespace Finance;

public class FinanceModule : IModule
{
    public string Name => "Finance";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration) { }

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/finance").WithTags("Finance").RequireAuthorization();
        group.MapGet("/fee-schedules", () => Results.Ok(Array.Empty<object>()));
    }
}

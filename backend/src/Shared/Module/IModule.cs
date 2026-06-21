using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Module;

public interface IModule
{
    string Name { get; }
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
    void RegisterEndpoints(IEndpointRouteBuilder endpoints);
}

public static class ModuleExtensions
{
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, IConfiguration configuration)
        where TModule : class, IModule
    {
        services.AddSingleton<IModule, TModule>();
        var instance = Activator.CreateInstance<TModule>();
        instance.RegisterServices(services, configuration);
        return services;
    }

    public static WebApplication MapModules(this WebApplication app)
    {
        var modules = app.Services.GetServices<IModule>();
        foreach (var module in modules)
        {
            module.RegisterEndpoints(app);
        }
        return app;
    }
}

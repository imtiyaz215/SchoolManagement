using Academic;
using Behaviour;
using Finance;
using Identity;
using Infrastructure;
using Infrastructure.Middleware;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Operations;
using Reference;
using Serilog;
using Shared.Module;
using Student;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddModule<IdentityModule>(builder.Configuration);
    builder.Services.AddModule<ReferenceModule>(builder.Configuration);
    builder.Services.AddModule<StudentModule>(builder.Configuration);
    builder.Services.AddModule<AcademicModule>(builder.Configuration);
    builder.Services.AddModule<BehaviourModule>(builder.Configuration);
    builder.Services.AddModule<FinanceModule>(builder.Configuration);
    builder.Services.AddModule<OperationsModule>(builder.Configuration);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "School Management API",
            Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = Array.Empty<string>()
        });
    });

    builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()));

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseMiddleware<TenantMiddleware>();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTime.UtcNow }));
    app.MapGet("/ready", async (SchoolDbContext db) =>
    {
        var canConnect = await db.Database.CanConnectAsync();
        return canConnect ? Results.Ok(new { status = "ready" }) : Results.StatusCode(503);
    });

    app.MapModules();

    await Seeder.SeedAsync(app.Services);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }

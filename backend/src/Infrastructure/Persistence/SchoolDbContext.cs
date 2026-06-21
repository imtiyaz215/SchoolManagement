using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Tenant;

namespace Infrastructure.Persistence;
// ApplyModuleConfigurations is in DbRegistration in the same namespace

public class SchoolDbContext : DbContext
{
    private readonly ITenantContext _tenant;

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options, ITenantContext tenant) : base(options)
    {
        _tenant = tenant;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DbRegistration.ApplyModuleConfigurations(modelBuilder);

        var method = typeof(SchoolDbContext)
            .GetMethod(nameof(ApplyTenantFilter),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var generic = method.MakeGenericMethod(entityType.ClrType);
                generic.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    private void ApplyTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : TenantEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => _tenant.SchoolId == null || e.SchoolId == _tenant.SchoolId);
    }

    public override int SaveChanges()
    {
        ApplyTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTimestamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = now;
            if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = now;
        }
    }
}

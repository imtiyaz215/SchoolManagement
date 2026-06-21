using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class SchoolDbContextDesignFactory : IDesignTimeDbContextFactory<SchoolDbContext>
{
    public SchoolDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SchoolDbContext>()
            .UseNpgsql("Host=localhost;Database=schoolmanagement;Username=postgres;Password=postgres")
            .Options;
        return new SchoolDbContext(options, new Shared.Tenant.TenantContext());
    }
}

using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
        await db.Database.MigrateAsync();

        if (!await db.Set<Role>().AnyAsync())
        {
            foreach (var name in Roles.All)
                db.Set<Role>().Add(new Role { Name = name });
            await db.SaveChangesAsync();
        }

        if (!await db.Set<School>().AnyAsync())
        {
            var school = new School
            {
                Name = "Demo School",
                Code = "DEMO",
                Email = "admin@demoschool.test",
                Phone = "+91-0000000000",
                Address = "Demo Address"
            };
            db.Set<School>().Add(school);
            await db.SaveChangesAsync();

            var adminRole = await db.Set<Role>().FirstAsync(r => r.Name == Roles.SchoolAdmin);
            db.Set<User>().Add(new User
            {
                SchoolId = school.Id,
                Email = "admin@demoschool.test",
                FullName = "School Admin",
                RoleId = adminRole.Id,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
            });
            await db.SaveChangesAsync();
        }
    }
}

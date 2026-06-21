using Behaviour.Infrastructure.Configurations;
using Academic.Infrastructure.Configurations;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reference.Infrastructure.Configurations;
using Student.Infrastructure.Configurations;

namespace Infrastructure.Persistence;

public static class DbRegistration
{
    public static IServiceCollection AddSchoolDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? Environment.GetEnvironmentVariable("DATABASE_URL")
            ?? "Host=localhost;Database=schoolmanagement;Port=5432;Username=postgres;Password=postgres";

        services.AddDbContext<DbContext, SchoolDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    public static void ApplyModuleConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.ApplyConfiguration(new AcademicSessionConfiguration());
        modelBuilder.ApplyConfiguration(new ClassGroupConfiguration());
        modelBuilder.ApplyConfiguration(new ClassConfiguration());
        modelBuilder.ApplyConfiguration(new SectionConfiguration());
        modelBuilder.ApplyConfiguration(new HouseConfiguration());
        modelBuilder.ApplyConfiguration(new ReligionConfiguration());
        modelBuilder.ApplyConfiguration(new CasteConfiguration());
        modelBuilder.ApplyConfiguration(new StateConfiguration());
        modelBuilder.ApplyConfiguration(new DistrictConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new ScholarTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QualificationConfiguration());
        modelBuilder.ApplyConfiguration(new OccupationConfiguration());
        modelBuilder.ApplyConfiguration(new ParentDesignationConfiguration());

        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new ParentConfiguration());
        modelBuilder.ApplyConfiguration(new StudentParentConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new StudentDocumentConfiguration());
        modelBuilder.ApplyConfiguration(new StudentStatusHistoryConfiguration());

        modelBuilder.ApplyConfiguration(new StudentEnrollmentConfiguration());
        modelBuilder.ApplyConfiguration(new EnrollmentHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new TeacherConfiguration());
        modelBuilder.ApplyConfiguration(new ClassInchargeAssignmentConfiguration());

        modelBuilder.ApplyConfiguration(new BehaviourTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new BehaviourItemConfiguration());
        modelBuilder.ApplyConfiguration(new BehaviourSheetConfiguration());
        modelBuilder.ApplyConfiguration(new BehaviourEntryConfiguration());
    }
}

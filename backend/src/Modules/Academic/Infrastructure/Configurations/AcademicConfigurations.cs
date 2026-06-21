using Academic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academic.Infrastructure.Configurations;

public class StudentEnrollmentConfiguration : IEntityTypeConfiguration<StudentEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentEnrollment> b)
    {
        b.ToTable("student_enrollments");
        b.HasKey(x => x.Id);
        b.Property(x => x.RollNumber).HasMaxLength(50);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        b.HasIndex(x => new
        {
            x.AcademicSessionId,
            x.ClassId,
            x.SectionId,
            x.RollNumber
        }).IsUnique().HasFilter(null);
        b.HasMany(x => x.History).WithOne(h => h.StudentEnrollment!).HasForeignKey(h => h.StudentEnrollmentId);
    }
}

public class EnrollmentHistoryConfiguration : IEntityTypeConfiguration<EnrollmentHistory>
{
    public void Configure(EntityTypeBuilder<EnrollmentHistory> b)
    {
        b.ToTable("enrollment_history");
        b.HasKey(x => x.Id);
        b.Property(x => x.ActionType).HasConversion<string>().HasMaxLength(30);
        b.Property(x => x.Reason).HasMaxLength(500);
    }
}

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> b)
    {
        b.ToTable("teachers");
        b.HasKey(x => x.Id);
        b.Property(x => x.Code).HasMaxLength(50).IsRequired();
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Mobile).HasMaxLength(30);
        b.Property(x => x.Email).HasMaxLength(300);
        b.Property(x => x.SignaturePath).HasMaxLength(500);
        b.HasIndex(x => new { x.SchoolId, x.Code }).IsUnique();
    }
}

public class ClassInchargeAssignmentConfiguration : IEntityTypeConfiguration<ClassInchargeAssignment>
{
    public void Configure(EntityTypeBuilder<ClassInchargeAssignment> b)
    {
        b.ToTable("class_incharge_assignments");
        b.HasKey(x => x.Id);
        b.HasOne(x => x.Teacher).WithMany().HasForeignKey(x => x.TeacherId);
        b.HasIndex(x => new
        {
            x.AcademicSessionId,
            x.ClassId,
            x.SectionId
        }).IsUnique();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student.Domain.Entities;
using StudentEntity = Student.Domain.Entities.StudentEntity;

namespace Student.Infrastructure.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> b)
    {
        b.ToTable("students");
        b.HasKey(x => x.Id);
        b.Property(x => x.AdmissionNumber).HasMaxLength(100).IsRequired();
        b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        b.Property(x => x.MiddleName).HasMaxLength(100);
        b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        b.Property(x => x.SecondaryLanguageName).HasMaxLength(100);
        b.Property(x => x.Phone).HasMaxLength(30);
        b.Property(x => x.SmsPhone).HasMaxLength(30);
        b.Property(x => x.Email).HasMaxLength(300);
        b.Property(x => x.BoardRegistrationNumber).HasMaxLength(100);
        b.Property(x => x.Remarks).HasMaxLength(1000);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        b.Property(x => x.Gender).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(x => x.AdmissionNumber).IsUnique();
        b.HasMany(x => x.Parents).WithOne(p => p.Student!).HasForeignKey(p => p.StudentId);
        b.HasMany(x => x.Addresses).WithOne(a => a.Student!).HasForeignKey(a => a.StudentId);
        b.HasMany(x => x.Documents).WithOne(d => d.Student!).HasForeignKey(d => d.StudentId);
        b.HasMany(x => x.StatusHistory).WithOne(h => h.Student!).HasForeignKey(h => h.StudentId);
    }
}

public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> b)
    {
        b.ToTable("parents");
        b.HasKey(x => x.Id);
        b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        b.Property(x => x.LastName).HasMaxLength(100);
        b.Property(x => x.Phone).HasMaxLength(50);
        b.Property(x => x.Email).HasMaxLength(300);
        b.Property(x => x.OfficeAddress).HasMaxLength(500);
    }
}

public class StudentParentConfiguration : IEntityTypeConfiguration<StudentParent>
{
    public void Configure(EntityTypeBuilder<StudentParent> b)
    {
        b.ToTable("student_parents");
        b.HasKey(x => new { x.StudentId, x.ParentId });
        b.Property(x => x.Relationship).HasConversion<string>().HasMaxLength(30);
        b.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId);
    }
}

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> b)
    {
        b.ToTable("addresses");
        b.HasKey(x => x.Id);
        b.Property(x => x.Line1).HasMaxLength(500).IsRequired();
        b.Property(x => x.AddressType).HasConversion<string>().HasMaxLength(30);
    }
}

public class StudentDocumentConfiguration : IEntityTypeConfiguration<StudentDocument>
{
    public void Configure(EntityTypeBuilder<StudentDocument> b)
    {
        b.ToTable("student_documents");
        b.HasKey(x => x.Id);
        b.Property(x => x.DocumentType).HasConversion<string>().HasMaxLength(30);
        b.Property(x => x.StoragePath).HasMaxLength(500).IsRequired();
    }
}

public class StudentStatusHistoryConfiguration : IEntityTypeConfiguration<StudentStatusHistory>
{
    public void Configure(EntityTypeBuilder<StudentStatusHistory> b)
    {
        b.ToTable("student_status_history");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        b.Property(x => x.Reason).HasMaxLength(500);
    }
}

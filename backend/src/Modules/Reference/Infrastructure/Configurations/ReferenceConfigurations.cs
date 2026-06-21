using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reference.Domain.Entities;

namespace Reference.Infrastructure.Configurations;

public class AcademicSessionConfiguration : IEntityTypeConfiguration<AcademicSession>
{
    public void Configure(EntityTypeBuilder<AcademicSession> b)
    {
        b.ToTable("academic_sessions");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.StartDate).IsRequired();
        b.Property(x => x.EndDate).IsRequired();
        b.HasIndex(x => new { x.SchoolId, x.Name }).IsUnique();
    }
}

public class ClassGroupConfiguration : IEntityTypeConfiguration<ClassGroup>
{
    public void Configure(EntityTypeBuilder<ClassGroup> b)
    {
        b.ToTable("class_groups");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.AdmissionPrefix).HasMaxLength(20);
        b.HasIndex(x => new { x.SchoolId, x.SequenceNo }).IsUnique();
    }
}

public class ClassConfiguration : IEntityTypeConfiguration<ClassEntity>
{
    public void Configure(EntityTypeBuilder<ClassEntity> b)
    {
        b.ToTable("classes");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.HasOne(x => x.ClassGroup).WithMany(g => g.Classes).HasForeignKey(x => x.ClassGroupId);
        b.HasIndex(x => new { x.ClassGroupId, x.SequenceNo }).IsUnique();
    }
}

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> b)
    {
        b.ToTable("sections");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(50).IsRequired();
        b.HasOne(x => x.Class).WithMany(c => c.Sections).HasForeignKey(x => x.ClassId);
        b.HasIndex(x => new { x.ClassId, x.Name }).IsUnique();
    }
}

public class HouseConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> b)
    {
        b.ToTable("houses");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.Color).HasMaxLength(30);
    }
}

public class ReligionConfiguration : IEntityTypeConfiguration<Religion>
{
    public void Configure(EntityTypeBuilder<Religion> b)
    {
        b.ToTable("religions");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}

public class CasteConfiguration : IEntityTypeConfiguration<Caste>
{
    public void Configure(EntityTypeBuilder<Caste> b)
    {
        b.ToTable("castes");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> b)
    {
        b.ToTable("states");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> b)
    {
        b.ToTable("districts");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.HasOne(x => x.State).WithMany().HasForeignKey(x => x.StateId);
    }
}

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> b)
    {
        b.ToTable("cities");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.PinCode).HasMaxLength(20);
        b.Property(x => x.StdCode).HasMaxLength(20);
        b.HasOne(x => x.District).WithMany().HasForeignKey(x => x.DistrictId);
    }
}

public class ScholarTypeConfiguration : IEntityTypeConfiguration<ScholarType>
{
    public void Configure(EntityTypeBuilder<ScholarType> b)
    {
        b.ToTable("scholar_types");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}

public class QualificationConfiguration : IEntityTypeConfiguration<Qualification>
{
    public void Configure(EntityTypeBuilder<Qualification> b) { b.ToTable("qualifications"); b.Property(x => x.Name).HasMaxLength(200).IsRequired(); }
}

public class OccupationConfiguration : IEntityTypeConfiguration<Occupation>
{
    public void Configure(EntityTypeBuilder<Occupation> b) { b.ToTable("occupations"); b.Property(x => x.Name).HasMaxLength(200).IsRequired(); }
}

public class ParentDesignationConfiguration : IEntityTypeConfiguration<ParentDesignation>
{
    public void Configure(EntityTypeBuilder<ParentDesignation> b) { b.ToTable("parent_designations"); b.Property(x => x.Name).HasMaxLength(200).IsRequired(); }
}

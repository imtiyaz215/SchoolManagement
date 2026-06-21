using Behaviour.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Behaviour.Infrastructure.Configurations;

public class BehaviourTemplateConfiguration : IEntityTypeConfiguration<BehaviourTemplate>
{
    public void Configure(EntityTypeBuilder<BehaviourTemplate> b)
    {
        b.ToTable("behaviour_templates");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(300).IsRequired();
        b.HasMany(x => x.Items).WithOne(i => i.Template!).HasForeignKey(i => i.TemplateId);
    }
}

public class BehaviourItemConfiguration : IEntityTypeConfiguration<BehaviourItem>
{
    public void Configure(EntityTypeBuilder<BehaviourItem> b)
    {
        b.ToTable("behaviour_items");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(300).IsRequired();
        b.Property(x => x.InputType).HasConversion<string>().HasMaxLength(30);
    }
}

public class BehaviourSheetConfiguration : IEntityTypeConfiguration<BehaviourSheet>
{
    public void Configure(EntityTypeBuilder<BehaviourSheet> b)
    {
        b.ToTable("behaviour_sheets");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        b.HasIndex(x => new { x.StudentId, x.Month, x.Year }).IsUnique();
        b.HasMany(x => x.Entries).WithOne(e => e.Sheet!).HasForeignKey(e => e.SheetId);
    }
}

public class BehaviourEntryConfiguration : IEntityTypeConfiguration<BehaviourEntry>
{
    public void Configure(EntityTypeBuilder<BehaviourEntry> b)
    {
        b.ToTable("behaviour_entries");
        b.HasKey(x => x.Id);
        b.Property(x => x.Value).HasMaxLength(500);
        b.HasOne(x => x.BehaviourItem).WithMany().HasForeignKey(x => x.BehaviourItemId);
    }
}

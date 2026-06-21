using Behaviour.Domain.Enums;
using Shared.Domain;

namespace Behaviour.Domain.Entities;

public class BehaviourTemplate : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<BehaviourItem> Items { get; set; } = new List<BehaviourItem>();
}

public class BehaviourItem : TenantEntity
{
    public Guid TemplateId { get; set; }
    public BehaviourTemplate? Template { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public BehaviourInputType InputType { get; set; }
}

public class BehaviourSheet : TenantEntity
{
    public Guid StudentId { get; set; }
    public Guid? ParentId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public BehaviourSheetStatus Status { get; set; } = BehaviourSheetStatus.Draft;
    public ICollection<BehaviourEntry> Entries { get; set; } = new List<BehaviourEntry>();
}

public class BehaviourEntry : TenantEntity
{
    public Guid SheetId { get; set; }
    public BehaviourSheet? Sheet { get; set; }
    public int DayNo { get; set; }
    public Guid BehaviourItemId { get; set; }
    public BehaviourItem? BehaviourItem { get; set; }
    public string? Value { get; set; }
}

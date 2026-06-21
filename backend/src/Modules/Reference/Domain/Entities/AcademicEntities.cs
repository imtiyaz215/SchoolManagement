using Shared.Domain;

namespace Reference.Domain.Entities;

public class AcademicSession : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class ClassGroup : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? AdmissionPrefix { get; set; }
    public int SequenceNo { get; set; }
    public ICollection<ClassEntity> Classes { get; set; } = new List<ClassEntity>();
}

public class ClassEntity : TenantEntity
{
    public Guid ClassGroupId { get; set; }
    public ClassGroup? ClassGroup { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SequenceNo { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}

public class Section : TenantEntity
{
    public Guid ClassId { get; set; }
    public ClassEntity? Class { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 40;
}

using Shared.Domain;

namespace Reference.Domain.Entities;

public class House : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}

public class ScholarType : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public class Religion : TenantEntity { public string Name { get; set; } = string.Empty; }

public class Caste : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsReservedCategory { get; set; }
}

public class Qualification : TenantEntity { public string Name { get; set; } = string.Empty; }
public class Occupation : TenantEntity { public string Name { get; set; } = string.Empty; }
public class ParentDesignation : TenantEntity { public string Name { get; set; } = string.Empty; }

public class State : BaseEntity { public string Name { get; set; } = string.Empty; public string? Code { get; set; } }
public class District : BaseEntity { public Guid StateId { get; set; } public State? State { get; set; } public string Name { get; set; } = string.Empty; }
public class City : BaseEntity
{
    public Guid DistrictId { get; set; }
    public District? District { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PinCode { get; set; }
    public string? StdCode { get; set; }
}

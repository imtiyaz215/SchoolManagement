namespace Shared.Tenant;

public sealed class TenantContext : ITenantContext
{
    public Guid? SchoolId { get; private set; }
    public Guid? UserId { get; private set; }

    public void SetSchool(Guid schoolId) => SchoolId = schoolId;
    public void SetUser(Guid userId) => UserId = userId;
}

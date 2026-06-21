namespace Shared.Tenant;

public interface ITenantContext
{
    Guid? SchoolId { get; }
    Guid? UserId { get; }
    bool HasSchool => SchoolId.HasValue;
    void SetSchool(Guid schoolId);
    void SetUser(Guid userId);
}

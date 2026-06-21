using Shared.Domain;

namespace Finance.Domain.Entities;

public class FeeSchedule : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public int Month { get; set; }
    public decimal Amount { get; set; }
}

public class StudentFeeAssignment : TenantEntity
{
    public Guid StudentEnrollmentId { get; set; }
    public Guid FeeScheduleId { get; set; }
    public FeeSchedule? FeeSchedule { get; set; }
}

using Shared.Domain;
using Student.Domain.Enums;

namespace Student.Domain.Entities;

public class StudentEntity : TenantEntity
{
    public string AdmissionNumber { get; set; } = string.Empty;
    public Guid? ScholarTypeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? SecondaryLanguageName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DOB { get; set; }
    public Guid? ReligionId { get; set; }
    public Guid? CasteId { get; set; }
    public string? Phone { get; set; }
    public string? SmsPhone { get; set; }
    public string? Email { get; set; }
    public string? BoardRegistrationNumber { get; set; }
    public string? Remarks { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public ICollection<StudentParent> Parents { get; set; } = new List<StudentParent>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<StudentDocument> Documents { get; set; } = new List<StudentDocument>();
    public ICollection<StudentStatusHistory> StatusHistory { get; set; } = new List<StudentStatusHistory>();
}

public class Parent : TenantEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public Guid? QualificationId { get; set; }
    public Guid? OccupationId { get; set; }
    public Guid? DesignationId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? OfficeAddress { get; set; }
}

public class StudentParent
{
    public Guid StudentId { get; set; }
    public StudentEntity? Student { get; set; }
    public Guid ParentId { get; set; }
    public Parent? Parent { get; set; }
    public ParentRelationship Relationship { get; set; }
}

public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public StudentEntity? Student { get; set; }
    public AddressType AddressType { get; set; }
    public string Line1 { get; set; } = string.Empty;
    public Guid CityId { get; set; }
}

public class StudentDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public StudentEntity? Student { get; set; }
    public DocumentType DocumentType { get; set; }
    public string StoragePath { get; set; } = string.Empty;
}

public class StudentStatusHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public StudentEntity? Student { get; set; }
    public StudentStatus Status { get; set; }
    public DateOnly EffectiveDate { get; set; }
    public string? Reason { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}

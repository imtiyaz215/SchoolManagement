using Student.Domain.Enums;

namespace Student.Contracts;

public record CreateStudentRequest(
    string AdmissionNumber,
    string FirstName,
    string? MiddleName,
    string LastName,
    Gender Gender,
    DateOnly DOB,
    Guid? ReligionId,
    Guid? CasteId,
    Guid? ScholarTypeId,
    string? Phone,
    string? Email,
    string? Remarks,
    List<CreateParentLink>? Parents,
    List<CreateAddressRequest>? Addresses);

public record CreateParentLink(
    string FirstName,
    string? LastName,
    string? Phone,
    string? Email,
    ParentRelationship Relationship);

public record CreateAddressRequest(
    AddressType AddressType,
    string Line1,
    Guid CityId);

public record StudentListItem(
    Guid Id,
    string AdmissionNumber,
    string FullName,
    Gender Gender,
    DateOnly DOB,
    string Status);

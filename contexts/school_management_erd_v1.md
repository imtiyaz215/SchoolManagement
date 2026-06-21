# School Management Platform — Detailed ERD Specification (v1.0)

## Objective
Define the relational model and ownership boundaries for the School Management Platform.

Principles:
- Multi-tenant via SchoolId
- Soft lifecycle (history, not deletes)
- Enrollment-driven academics
- Configurable reference data

# Identity
User(Id, SchoolId, Email, PasswordHash, RoleId)
Role(Id, Name)

# Reference
AcademicSession(Id, SchoolId, Name, StartDate, EndDate, IsActive)
ClassGroup(Id, SchoolId, Name, AdmissionPrefix, SequenceNo)
Class(Id, SchoolId, ClassGroupId, Name, SequenceNo)
Section(Id, ClassId, Name, Capacity)
House(Id, SchoolId, Name, Color)
ScholarType(Id, SchoolId, Name, IsDefault)
Religion(Id, SchoolId, Name)
Caste(Id, SchoolId, Name, IsReservedCategory)
Qualification(Id, SchoolId, Name)
Occupation(Id, SchoolId, Name)
ParentDesignation(Id, SchoolId, Name)

State(Id, Name)
District(Id, StateId, Name)
City(Id, DistrictId, Name, PinCode, StdCode)

# Student
Student(
 Id,
 SchoolId,
 AdmissionNumber,
 ScholarTypeId,
 FirstName,
 MiddleName,
 LastName,
 Gender,
 DOB,
 ReligionId,
 CasteId,
 Phone,
 Email
)

Parent(
 Id,
 SchoolId,
 QualificationId,
 OccupationId,
 DesignationId,
 Name,
 Phone,
 Email
)

StudentParent(
 StudentId,
 ParentId,
 Relationship
)

Address(
 Id,
 StudentId,
 AddressType,
 Line1,
 CityId
)

StudentDocument(
 Id,
 StudentId,
 DocumentType,
 StoragePath
)

StudentStatusHistory(
 Id,
 StudentId,
 Status,
 EffectiveDate,
 Reason
)

# Academic
StudentEnrollment(
 Id,
 StudentId,
 AcademicSessionId,
 ClassId,
 SectionId,
 RollNumber,
 Status
)

EnrollmentHistory(
 Id,
 StudentEnrollmentId,
 ActionType,
 OldClassId,
 OldSectionId,
 NewClassId,
 NewSectionId,
 ChangedAt
)

Teacher(
 Id,
 Code,
 Name,
 Mobile,
 Email,
 SignaturePath
)

ClassInchargeAssignment(
 Id,
 AcademicSessionId,
 ClassId,
 SectionId,
 TeacherId
)

# Finance
FeeSchedule(
 Id,
 SchoolId,
 Name,
 Month,
 Amount
)

StudentFeeAssignment(
 Id,
 StudentEnrollmentId,
 FeeScheduleId
)

# Operations
CertificateTemplate(
 Id,
 SchoolId,
 Name,
 TemplatePath
)

StudentCertificate(
 Id,
 StudentId,
 CertificateTemplateId,
 CertificateNumber,
 IssueDate
)

GatePass(
 Id,
 GatePassNumber,
 Type,
 IssuedAt,
 Reason
)

StudentGatePass(
 Id,
 GatePassId,
 StudentId,
 Relation,
 AccompaniedBy
)

# Behaviour
BehaviourTemplate(Id, SchoolId, Name)
BehaviourItem(Id, TemplateId, Name, DisplayOrder, InputType)
BehaviourSheet(Id, StudentId, Month, Year, ParentId, Status)
BehaviourEntry(Id, SheetId, DayNo, BehaviourItemId, Value)

## Constraints
- One active enrollment per student
- RollNumber unique per Session+Class+Section
- Never delete Student
- Address references City only

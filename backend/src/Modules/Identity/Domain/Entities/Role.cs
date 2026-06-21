using Shared.Domain;

namespace Identity.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string SchoolAdmin = "SchoolAdmin";
    public const string Teacher = "Teacher";
    public const string Parent = "Parent";
    public const string Student = "Student";

    public static readonly string[] All = { SuperAdmin, SchoolAdmin, Teacher, Parent, Student };
}

# School Management Platform — Backend Implementation Blueprint (v1.0)

## Goal
Build a production-grade backend for School Management using ASP.NET Core (.NET 9).

Primary goals:
- Modular Monolith
- Multi-tenant
- API-first
- Mobile + Web support
- Maintainable codebase

---

# Solution Structure

SchoolManagement/

src/

Api/

Modules/
  Identity/
  Reference/
  Student/
  Academic/
  Finance/
  Operations/
  Behaviour/

Shared/
Infrastructure/

tests/

---

# Project Structure

Each module:

Module/

Domain/
  Entities/
  Enums/
  ValueObjects/

Application/
  Services/
  Commands/
  Queries/
  DTO/
  Validators/

Infrastructure/
  Persistence/
  Configurations/

Contracts/

---

# API Layer

Minimal APIs only.

Example:

MapGroup("/students")
MapGroup("/academic")
MapGroup("/behaviour")

Rules:

- No business logic in endpoints
- Validation before service execution
- Return ProblemDetails

---

# Dependency Injection

Program.cs:

AddIdentityModule()
AddReferenceModule()
AddStudentModule()
AddAcademicModule()
AddFinanceModule()
AddOperationsModule()
AddBehaviourModule()

---

# Database

PostgreSQL

EF Core Code First

DbContexts:

SchoolDbContext

Avoid multiple contexts initially.

Configurations:

IEntityTypeConfiguration

Naming:

snake_case

---

# Tenant Isolation

SchoolContext:

CurrentSchoolId
CurrentUserId

Global Query Filters:

SchoolId

---

# Authentication

ASP.NET Identity
JWT
Refresh Tokens

Roles:

SuperAdmin
SchoolAdmin
Teacher
Parent
Student

---

# Authorization

Policies:

Students.Read
Students.Write
Academic.Write
Finance.Read
Behaviour.Submit

---

# Validation

Use FluentValidation.

Structure:

Validators/

Examples:

CreateStudentValidator
CreateEnrollmentValidator

---

# Logging

Serilog

Include:

CorrelationId
UserId
SchoolId

---

# Caching

MemoryCache initially.

Cache:

Reference Data
Class Lists

---

# Storage

Abstraction:

IFileStorage

Implementations:

S3Storage
LocalStorage

---

# Notifications

Abstraction:

INotificationService

Implementations:

Firebase
Email

---

# Background Jobs

Hangfire

Jobs:

Attendance Summary
Behaviour Reminder
Certificate Generation

---

# Testing

xUnit
FluentAssertions

Test folders:

Unit/
Integration/

---

# Observability

OpenTelemetry
HealthChecks

Endpoints:

/health
/ready

---

# Module Delivery Order

Phase 1
Reference
Identity
Student
Academic

Phase 2
Behaviour
Certificates

Phase 3
Finance
Notifications

Phase 4
Parent Mobile

---

# Initial Endpoints

POST /auth/login
POST /students
GET /students
POST /enrollments
POST /behaviour/submit
GET /certificates

---

# Rules

Do not:

- expose entities directly
- put SQL in endpoints
- create repository for every entity
- create microservices

Prefer:

Endpoint -> Service -> EF

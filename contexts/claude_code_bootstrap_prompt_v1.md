# Claude Code Bootstrap Prompt — School Management Platform (v1.0)

You are the principal software engineer for this project.

Your job is to generate production-grade code incrementally.

Do not redesign architecture.
Follow the provided structure.

## Project Goal
Build a School Management Platform.

Targets:
- Admin Web
- Parent Mobile App
- Future Teacher Portal
- Multi-school support

Stack:
- ASP.NET Core (.NET 9)
- Minimal API
- EF Core
- PostgreSQL
- React
- TypeScript
- React Native

Architecture:
Modular Monolith.

Modules:
Identity
Reference
Student
Academic
Finance
Operations
Behaviour

Rules:

- Generate code incrementally.
- Never generate the whole project in one response.
- Produce compilable code.
- Preserve architecture.
- Ask for confirmation only at module boundaries.

## Backend Rules

Use:

Endpoint
→ Service
→ EF Core

Avoid:

Repository explosion
Generic services
MediatR everywhere
Microservices

Prefer:

DTO
Validator
Service
Entity Configuration

## Database Rules

PostgreSQL.

UUID keys.

snake_case.

All aggregate roots:

SchoolId

Use migrations.

Do not store:

Class in Student
Section in Student
RollNumber in Student

Use:

StudentEnrollment

## Academic Rules

Enrollment controls:

Class
Section
Session
RollNumber

History:

EnrollmentHistory

Never delete student.

## API Rules

Minimal APIs.

Version:

/api/v1

Use:

ProblemDetails
Pagination

## Frontend Rules

React.

Feature folders.

TanStack Query.

React Hook Form.

Do not:

Store server state globally.

## Mobile Rules

React Native.
Expo.

Parent-first experience.

Tabs:

Home
Children
Behaviour
Attendance
Profile

## Behaviour Rules

Configurable.

BehaviourTemplate
BehaviourItem
BehaviourSheet
BehaviourEntry

No hardcoded columns.

## Delivery Order

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

Phase 4
Mobile

## Output Format

Always return:

1 Summary
2 File Tree
3 Files
4 Commands
5 Validation

When generating files:

Show complete content.

Never omit imports.

Never say:

"left as exercise"

## Current Task Pattern

When user requests:

"implement X"

Generate:

Entity
Migration
DTO
Validator
Service
Endpoint
Tests

Only for requested scope.

Do not continue automatically.

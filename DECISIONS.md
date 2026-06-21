# Decisions

## ADR-001 Modular monolith over microservices

Single deployment unit, single DB, shared kernel. Modules are bounded projects with explicit dependencies. Easier to evolve, easier to test, easier to operate. Revisit when team > 10 or scale demands it.

## ADR-002 .NET 9 Minimal API

Less ceremony than MVC controllers, native to OpenAPI, fast. Modules expose endpoints via an `IModule` contract so DI and routing are uniform.

## ADR-003 EF Core code-first, snake_case

Postgres convention. Each module owns its `IEntityTypeConfiguration`. Migrations live in `Infrastructure`.

## ADR-004 Tenant isolation via global query filter + JWT claim

Every aggregate except global config has `SchoolId`. The JWT carries `school_id`. A middleware sets `ITenantContext.SchoolId` and EF applies `HasQueryFilter(e => e.SchoolId == tenant.SchoolId)`. No per-request filter clauses needed.

## ADR-005 Service classes, no MediatR

Services implement narrow interfaces. Endpoints call services. Repositories only where a real abstraction is needed (none yet). Validation lives in service methods returning `Result<T>`.

## ADR-006 String-stored enums

All enums map to `string` columns to keep schema tolerant of renames.

## ADR-007 React + Vite + TanStack Query

Vite for dev speed. TanStack Query for server state. React Hook Form + Zod reserved for the admissions wizard. Auth via React Context (no Redux/Zustand for now).

## ADR-008 React Native + Expo

Expo for build simplicity and OTA updates later. Single codebase for Android + iOS. SecureStore for tokens.

## ADR-009 JWT + Refresh Tokens

Access token 60 min, refresh 14 days, rotated on use. No cookies for the API — the web and mobile clients manage storage themselves.

## ADR-010 Interface-driven storage + notifications

`IFileStorage` and `INotificationService` interfaces in `Shared`. S3 + Firebase implementations land in `Infrastructure`. Local + Null implementations exist so dev environments don't need external services.

## ADR-011 No microservice split for Finance / Operations yet

Both modules are stubbed in v1 (entities + endpoints). Promote to full services after the schema is observed in production.

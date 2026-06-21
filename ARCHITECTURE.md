# Architecture

## Modular monolith + Clean Architecture (lite)

```
src/
├── Api/                  Minimal API host (composition root)
├── Shared/               Cross-cutting types (no business logic)
│   ├── Domain/           BaseEntity, TenantEntity
│   ├── Results/          Result<T>, PagedResponse
│   ├── Exceptions/       DomainException, NotFoundException
│   ├── Tenant/           ITenantContext
│   ├── Storage/          IFileStorage
│   ├── Notifications/    INotificationService
│   └── Module/           IModule contract (DI + endpoints)
├── Infrastructure/       EF DbContext, middleware, JWT, seeding
└── Modules/
    ├── Identity/
    │   ├── Domain/       School, Role, User
    │   ├── Application/  AuthService, TokenService, UserService
    │   ├── Infrastructure/ EF configurations
    │   ├── Contracts/    DTOs
    │   └── IdentityModule.cs
    ├── Reference/        ... same shape
    ├── Student/
    ├── Academic/
    ├── Behaviour/
    ├── Finance/
    └── Operations/
```

Each module is independent. They share `Shared` + `Infrastructure` only.

## Multi-tenancy

Every aggregate except global config has `SchoolId`. The JWT carries `school_id`.
A middleware (`TenantMiddleware`) reads the claim and sets `ITenantContext.SchoolId`.
`SchoolDbContext` applies a global EF query filter so every query is automatically scoped.

```csharp
HasQueryFilter(e => _tenant.SchoolId == null || e.SchoolId == _tenant.SchoolId);
```

The `null` branch is reserved for `SuperAdmin` cross-school operations.

## Why modular monolith

- One process, one deployment unit, one transaction boundary.
- Clear boundaries via projects + namespaces; physical separation later is a refactor, not a rewrite.
- Shared kernel (`Shared`) keeps modules honest about coupling.

## Endpoint → Service → EF

```csharp
group.MapPost("/students", async (CreateStudentRequest req, IStudentService svc, CancellationToken ct) => {
    var result = await svc.CreateAsync(req, ct);
    return result.IsSuccess ? Results.Created(...) : Results.BadRequest(result.Error);
});
```

No SQL in endpoints. No business logic in endpoints. No repositories unless the abstraction actually buys something.

## Naming & schema

- C#: PascalCase.
- DB tables/columns: snake_case (configured per `IEntityTypeConfiguration`).
- Enum storage: string columns for forward-compatibility.

## Auth

JWT + rotating refresh tokens, BCrypt password hashing. Refresh tokens stored on the user row; rotated on every use.

## What we deliberately avoided

- ❌ CQRS everywhere
- ❌ MediatR / pipeline behaviors
- ❌ Repository per entity
- ❌ Microservices
- ❌ DDD ceremony (no aggregates/services distinction, no domain events for now)

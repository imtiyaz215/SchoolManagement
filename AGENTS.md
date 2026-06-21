# Agents / Contributors

## Build order (incremental delivery)

1. Reference data (academic sessions, classes, sections, religions, geography)
2. Identity (schools, users, login)
3. Student (admission, parents, addresses, documents, lifecycle)
4. Academic (enrollments, promotion, section transfer, class incharge)
5. Behaviour (templates, monthly sheets, parent submission)
6. Finance (fee schedules, assignments, payments)
7. Operations (certificates, gate pass, attendance)
8. Mobile parent app (auth, children, behaviour submission)

## Conventions

- Backend: endpoint → service → EF. No SQL in endpoints.
- All new modules follow the `Domain / Application / Infrastructure / Contracts` layout.
- Multi-tenant: every aggregate has `SchoolId`. Global query filter handles isolation.
- Enums map to strings.
- Tables/columns are snake_case.
- DTOs live in `Contracts/`, never expose entities directly.
- Frontend: TanStack Query for server state. Routes are file-name mapped from the backend modules.
- Mobile: tabs for top-level nav. SecureStore for tokens.

## Don't

- Don't create a repository per entity. Use `DbContext.Set<T>()`.
- Don't introduce MediatR or a CQRS pipeline.
- Don't store roll number or class on `Student`. They belong to `StudentEnrollment`.
- Don't delete students. Move to `Left`/`Transferred`/`Graduated` instead.
- Don't hardcode religion/caste values; always seed via the Reference module.

# School Management Platform

Production-grade school management platform replacing the legacy school system.

Targets: Web Admin Portal · Parent Mobile App · Future Teacher Portal · Multi-school SaaS.

## Stack

| Layer    | Tech                                       |
| -------- | ------------------------------------------ |
| Backend  | ASP.NET Core .NET 9, Minimal API, EF Core  |
| Database | PostgreSQL 16                              |
| Web      | React + TypeScript + Vite + TanStack Query |
| Mobile   | React Native + Expo                        |
| Auth     | JWT + Refresh Tokens                       |
| Storage  | S3-compatible (interface)                  |
| Notify   | Firebase (interface)                       |
| Deploy   | Docker, docker-compose                     |

## Repository

```
school-management/
├── backend/      ASP.NET Core modular monolith
├── web/          React admin portal
├── mobile/       React Native + Expo parent app
├── infra/        Docker compose, Dockerfile
└── contexts/     Source-of-truth specs (read-only)
```

## Quick start

### 1. Start Postgres + pgAdmin + API

```bash
cd infra
docker compose up -d
```

API: http://localhost:5000  ·  Swagger: http://localhost:5000/swagger
pgAdmin: http://localhost:5050 (admin@admin.com / admin)

### 2. Run the API locally (optional)

```bash
cd backend
dotnet restore
dotnet run --project src/Api
```

Migrations apply automatically on startup; seed data creates a demo school:
- School: `Demo School`
- Admin login: `admin@demoschool.test` / `Admin@123`

### 3. Run the web admin

```bash
cd web
npm install
npm run dev
```

Open http://localhost:5173

### 4. Run the mobile parent app

```bash
cd mobile
npm install
npx expo start
```

## Modules

| Module      | Responsibility                                          |
| ----------- | ------------------------------------------------------- |
| Identity    | Auth, schools, users, roles                             |
| Reference   | Academic structure, lookups, geography                  |
| Student     | Students, parents, addresses, documents, lifecycle      |
| Academic    | Enrollments, history, promotions, teachers, incharge     |
| Behaviour   | Templates, sheets, entries (parent-driven)              |
| Finance     | Fee schedules, assignments (stub)                        |
| Operations  | Certificates, gate pass (stub)                            |

## Auth

- JWT access tokens (60 min default) + rotating refresh tokens (14 days).
- Tenant (`school_id`) claim drives automatic data isolation via EF Core global query filters.
- Roles: `SuperAdmin`, `SchoolAdmin`, `Teacher`, `Parent`, `Student`.

## API surface

All endpoints under `/api/v1`.

| Group           | Routes                                                        |
| --------------- | ------------------------------------------------------------- |
| `/auth`         | `POST /login`, `POST /refresh`, `GET /me`                    |
| `/schools`      | `GET/POST /`                                                  |
| `/users`        | `GET/POST /`                                                  |
| `/academic`     | `/sessions`, `/class-groups`, `/classes`, `/sections`         |
| `/reference`    | `/religions`, `/castes`, `/houses`, `/states`, `/districts`... |
| `/students`     | `GET/POST /`, `GET /{id}`, `PATCH /{id}/status`               |
| `/academic/enrollments` | `POST /`, `/promote`, `/section-transfer`, `/history` |
| `/behaviour`    | `/templates`, `/sheets`, `/review`                            |
| `/finance`      | `/fee-schedules` (stub)                                       |
| `/operations`   | `/certificates`, `/gatepass` (stub)                           |

## Deliveries

- Phase 1 ✅ Reference, Identity, Student, Academic
- Phase 2 ✅ Behaviour
- Phase 3 ◻ Finance, Operations (scaffolded)
- Phase 4 ◻ Mobile parent (shell complete, screens stubbed)

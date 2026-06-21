# School Management Platform — Monorepo Folder Structure + Commands (v1.0)

## Goal
Single repository for backend, web, mobile, infrastructure and docs.

Repository:

school-management/

├── README.md
├── AGENTS.md
├── ARCHITECTURE.md
├── DECISIONS.md
├── docker-compose.yml
├── .github/
├── docs/
│
├── backend/
│   ├── SchoolManagement.sln
│   ├── src/
│   │
│   │   ├── Api/
│   │   ├── Modules/
│   │   │
│   │   │   ├── Identity/
│   │   │   ├── Reference/
│   │   │   ├── Student/
│   │   │   ├── Academic/
│   │   │   ├── Finance/
│   │   │   ├── Operations/
│   │   │   └── Behaviour/
│   │   │
│   │   ├── Shared/
│   │   └── Infrastructure/
│   │
│   └── tests/
│
├── web/
│   ├── src/
│   ├── public/
│   └── package.json
│
├── mobile/
│   ├── app/
│   ├── src/
│   └── package.json
│
├── scripts/
├── infra/
└── tools/

---

# Bootstrap Commands

## Create Root

mkdir school-management
cd school-management

---

# Backend

mkdir backend
cd backend

dotnet new sln

mkdir src
mkdir tests

cd src

mkdir Api
mkdir Modules
mkdir Shared
mkdir Infrastructure

cd ..

---

# Create Web

npm create vite@latest web

Choose:

React
TypeScript

Install:

npm install

---

# Create Mobile

npx create-expo-app mobile

Install:

npm install

---

# PostgreSQL

Docker:

mkdir infra

Create:

docker-compose.yml

Services:

postgres
pgadmin

Start:

docker compose up -d

---

# EF Core

Install:

dotnet tool install --global dotnet-ef

Create:

dotnet ef migrations add Initial

dotnet ef database update

---

# Backend Run

cd backend

dotnet restore

dotnet build

dotnet run

---

# Web Run

cd web

npm install
npm run dev

---

# Mobile Run

cd mobile

npm install
npx expo start

---

# Testing

Backend:

dotnet test

Web:

npm run test

---

# Formatting

Backend:

dotnet format

Frontend:

npm run lint

---

# Environment

backend/.env
web/.env
mobile/.env

Variables:

DATABASE_URL
JWT_KEY
S3_URL
FIREBASE_KEY

---

# Docker

Build:

docker compose build

Run:

docker compose up

Stop:

docker compose down

---

# CI

.github/workflows/

backend.yml
frontend.yml
mobile.yml

Pipeline:

restore
build
test
publish

---

# VS Code

Recommended:

C#
ESLint
Prettier
Docker
GitLens

Workspace:

school-management.code-workspace

---

# Delivery Order

1 Reference
2 Identity
3 Student
4 Academic
5 Behaviour
6 Finance
7 Operations
8 Mobile

---

# Rules

Do not:

- split into microservices
- create multiple repos
- duplicate DTOs

Prefer:

single db
single backend
shared contracts

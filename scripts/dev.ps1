#!/usr/bin/env pwsh
# Dev startup: brings up Postgres via docker, runs API + web in parallel terminals.
# Usage: pwsh scripts/dev.ps1

$ErrorActionPreference = 'Stop'

Write-Host "Starting Postgres + pgAdmin..." -ForegroundColor Cyan
docker compose -f infra/docker-compose.yml up -d postgres pgadmin

Write-Host "Starting API..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd backend; dotnet run --project src/Api" -WindowStyle Normal

Write-Host "Starting Web..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd web; npm run dev" -WindowStyle Normal

Write-Host "Done. API at http://localhost:5000 (Swagger: /swagger), Web at http://localhost:5173, pgAdmin at http://localhost:5050" -ForegroundColor Green
Write-Host "Demo login: admin@demoschool.test / Admin@123"

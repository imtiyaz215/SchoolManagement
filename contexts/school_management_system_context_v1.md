# School Management Platform — System Context (v1.0)

This document consolidates the discovered requirements and architecture baseline.

## Purpose
Build a production-grade School Management Platform replacing a legacy school system.

Targets:
- Web Admin Portal
- Parent Mobile App
- Future Teacher Portal
- Multi-school support

## Stack
Backend: ASP.NET Core (.NET 9), Minimal API, EF Core, PostgreSQL
Web: React + TypeScript
Mobile: React Native + Expo
Auth: ASP.NET Identity + JWT
Storage: S3-compatible

## Architecture
Modular Monolith.
Modules:
- Identity
- Reference
- Student
- Academic
- Finance
- Operations
- Behaviour

## Confirmed Modeling Decisions
- Student is permanent identity
- Roll Number belongs to Enrollment
- Class and Section belong to Enrollment
- Student is never deleted
- Address references City
- Geography: State -> District -> City
- Parents are separate entities
- Certificates support templates + issuance
- Behaviour is configurable

## Core Entities
Student
Parent
StudentParent
Address
AcademicSession
ClassGroup
Class
Section
StudentEnrollment
EnrollmentHistory
FeeSchedule
CertificateTemplate
StudentCertificate
GatePass
BehaviourTemplate
BehaviourSheet
BehaviourEntry

## Build Order
1. Reference
2. Student
3. Academic
4. Behaviour
5. Finance
6. Operations
7. Mobile

## Open Validation
- Fees
- Attendance
- Health
- Subjects
- Reports

# School Management Platform — Frontend Screen Map (v1.0)

## Goal
Define frontend navigation, screens, routes, folders, and UX boundaries.

Platforms:
- Admin Web (React)
- Parent Mobile (React Native)
- Future Teacher Portal

---

# Admin Portal

Route:
/

Modules:

Dashboard
Admissions
Students
Academic
Finance
Behaviour
Certificates
Operations
Reports
Settings

---

# Authentication

/login
/forgot-password
/change-password

Screens:
- Login
- Session Selection

---

# Dashboard

/dashboard

Widgets:
- Total Students
- Attendance Summary
- Behaviour Pending
- Certificates Issued
- Recent Activity

---

# Admissions

/admissions

Screens:

/admissions/new
/admissions/list
/admissions/details/:id

Wizard:

Step 1 Student
Step 2 Parents
Step 3 Address
Step 4 Documents
Step 5 Review

---

# Students

/students

Screens:

Student List
Student Detail
Student Timeline

Tabs:

Profile
Parents
Enrollment
Documents
Behaviour
Certificates
Gate Pass
History

---

# Academic

/academic

Sections:

Sessions
Class Groups
Classes
Sections
Enrollments
Promotion
Section Transfer
House Assignment
Class Incharge

Routes:

/academic/sessions
/academic/classes
/academic/enrollments

---

# Reference

/reference

Screens:

Religion
Caste
Qualification
Occupation
Designation
Scholar Type
House
State
District
City

---

# Behaviour

/behaviour

Screens:

Templates
Submissions
Review
Reports

Routes:

/behaviour/templates
/behaviour/sheets

---

# Certificates

/certificates

Screens:

Templates
Issue Certificate
Issued History

---

# Finance

/finance

Screens:

Fee Schedule
Student Fees
Payment History

---

# Operations

/operations

Screens:

Attendance
Gate Pass
Notifications

---

# Reports

/reports

Screens:

Student Reports
Behaviour Reports
Fee Reports
Certificate Reports

---

# Settings

/settings

Screens:

Users
Roles
School Settings
Storage
Notifications

---

# Parent Mobile App

Tabs:

Home
Children
Behaviour
Attendance
Profile

---

# Parent Screens

Splash
Login
OTP

Home

Child Switcher

Child Detail

Behaviour Sheet

Monthly History

Notifications

Certificates

Profile

---

# Behaviour Mobile Flow

Notification
→ Open Behaviour
→ Fill
→ Submit
→ Confirmation

---

# Shared Components

components/

DataTable
FormField
DatePicker
SearchInput
Dialog
Card
Timeline
FileUpload

---

# React Structure

src/

routes/
pages/
components/
features/
services/
hooks/
store/
utils/

---

# State Management

TanStack Query

Local:

React State

Global:

Auth
School
Theme

---

# Forms

React Hook Form
Zod

---

# Design Rules

- Mobile-first forms
- Wizard for admissions
- Avoid giant screens
- Lazy routes
- Server pagination

---

# Delivery Order

Phase 1
Login
Admissions
Students

Phase 2
Academic
Behaviour

Phase 3
Finance
Reports

Phase 4
Parent App

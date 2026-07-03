# Examination System API

A backend system for managing online quizzes and exams, built with ASP.NET Core (.NET 8) using Clean Architecture principles.

## Overview

The Examination System provides the backend infrastructure for an e-learning platform where students enroll in diplomas/courses, take quizzes, and receive automated scoring, while admins manage content and track performance.

Core capabilities include:

- **Authentication & Authorization** – JWT-based auth with refresh tokens, role-based access (Student/Admin), OTP verification, and password reset flows.
- **Diplomas & Enrollments** – Admins create diplomas (course tracks); students enroll and track their progress.
- **Quiz Management** – Admins create quizzes with questions and multiple-choice options, configure duration and passing score.
- **Quiz Attempts** – Students start a quiz attempt, answer questions within a time limit, and submit for automatic scoring. The system enforces attempt limits and prevents multiple simultaneous in-progress attempts.
- **Scoring & Reporting** – Automatic score calculation on submission, with detailed attempt results for students and aggregated statistics for admins (pass rates, attempt history, per-quiz analytics).
- **Email Notifications** – Transactional emails (OTP, password reset) via MailKit/MimeKit.

## Architecture

The solution follows **Clean Architecture**, split into four layers:

```
ExaminationSystem.Domain          → Entities, enums, domain errors, repository interfaces
ExaminationSystem.Application     → CQRS features (Commands/Queries) via MediatR, validation, DTOs
ExaminationSystem.Infrastructure  → EF Core persistence, repositories, external services (email, JWT, caching)
ExaminationSystem.Api             → Controllers, middlewares, DI wiring, Swagger
```

Each feature (e.g. Quizzes, Attempts, Diplomas, Users) is organized under `Application/Features/<Feature>/Commands` and `Queries`, following a vertical-slice style within the CQRS pattern — each handler is a self-contained use case.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 (Web API) |
| ORM | Entity Framework Core 8 (SQL Server) |
| Auth | ASP.NET Identity + JWT Bearer |
| CQRS / Mediator | MediatR |
| Validation | FluentValidation |
| Email | MailKit / MimeKit |
| Logging | Serilog |
| API Docs | Swashbuckle (Swagger) |
| Caching | In-memory caching (OTP flow) |

## Key Design Decisions

- **Repository + Unit of Work pattern** via a generic `IGeneralRepository<T>` for common data access, with dedicated repositories for more complex aggregates (Diploma, Quiz).
- **Result pattern** (`Result<T>` / `Error`) instead of throwing exceptions for expected failure cases, keeping error handling explicit and consistent across handlers.
- **Pipeline behaviors** (MediatR) for cross-cutting concerns such as validation.
- **Attempt lifecycle enforcement** — quiz attempts move through defined states (in-progress, submitted, timed-out) with domain rules preventing invalid transitions (e.g., double submission, exceeding max attempts).

## Getting Started

1. Update the connection string in `appsettings.json` / `appsettings.Development.json`.
2. Apply EF Core migrations:
   ```
   dotnet ef database update --project ExaminationSystem.Infrastructure --startup-project ExaminationSystem.Api
   ```
3. Run the API:
   ```
   dotnet run --project ExaminationSystem.Api
   ```
4. Browse the API via Swagger at `/swagger`.

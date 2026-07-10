# CourseLedger

Academic Management System for managing courses, students, academic records, employees, and roles.

## Features

- **Courses Management**: Full CRUD for course information, descriptions, and fees
- **Students Management**: Full CRUD for student records (manual ID entry)
- **Academic Records**: View, create, edit, bulk-edit, and delete student grades
- **Employee Management**: Create, edit, and delete employees with role assignments
- **Roles Management**: CRUD for job titles (manual ID assignment)

## Technology Stack

- ASP.NET Core 8.0 MVC
- Entity Framework Core 8.0
- **PostgreSQL** (Npgsql) — local Postgres or [Neon](https://neon.tech) for production
- Bootstrap 5 with custom dark theme
- Inter + JetBrains Mono typography

## Local Development

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL 14+ (local install, or a free Neon dev branch)
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository and navigate to the project:
```bash
cd CourseLedger
```

2. Create a local database (if using local Postgres):
```sql
CREATE DATABASE courseledger;
```

3. Update `appsettings.Development.json` if your credentials differ:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=courseledger;Username=postgres;Password=postgres"
  }
}
```

4. Restore and run:
```bash
dotnet restore
dotnet run --launch-profile http
```

5. Open **http://localhost:5062**

The app applies pending EF Core migrations automatically on startup (`Database.Migrate()`), then seeds sample data if the database is empty.

### Database Migrations

Schema changes require:

```bash
dotnet ef migrations add <MigrationName> --project CourseLedger
```

Migrations are applied automatically on the next app startup, or manually:

```bash
dotnet ef database update --project CourseLedger
```

## Deployment (Render + Neon)

The recommended free long-term stack:

| Service | Role |
|---------|------|
| [Render](https://render.com) | Hosts the ASP.NET Core web app (free tier) |
| [Neon](https://neon.tech) | Serverless PostgreSQL database (free tier) |

### Neon setup

1. Create a project at [neon.tech](https://neon.tech)
2. Copy the connection string from the Neon dashboard
3. Convert to Npgsql key-value format if needed:
   ```
   Host=ep-xxx.region.aws.neon.tech;Database=neondb;Username=...;Password=...;SSL Mode=Require
   ```
   Neon also provides a `postgresql://` URI — Npgsql accepts that format directly.

### Render setup

1. Connect your GitHub repository
2. Use the included `render.yaml` blueprint (or create a Web Service manually)
3. Set the environment variable:
   - `ConnectionStrings__DefaultConnection` = your Neon connection string (with `SSL Mode=Require`)
4. Deploy

> Do **not** commit real Neon credentials. Supply them only via Render environment variables.

### Railway (secondary option)

`railway.json` remains available as an alternate deploy target. Set `ConnectionStrings__DefaultConnection` to your Postgres connection string in Railway environment variables.

## Default Seed Data

- 4 sample courses (CST8256–CST8285)
- 5 sample students (S001–S005)
- 10 academic records with grades
- 3 roles: Administrator, Instructor, Staff
- 3 employees with assigned roles

## Running Tests

```bash
dotnet test CourseLedger.Tests
```

## PostgreSQL Notes

- **Case-sensitive string comparisons**: Postgres compares strings case-sensitively by default. `admin` and `Admin` are different usernames. SQL Server (with default collation) treated them as equal.
- **Course delete cascade**: Deleting a course still removes related academic records in application code (`CoursesController.DeleteConfirmed`).
- **No raw SQL**: The app uses EF Core LINQ only — no T-SQL or Postgres-specific SQL in controllers.

## Known Limitations

- **No authentication** — the app is open by design; employee passwords are stored but not used for login
- **No API/REST endpoints** — server-rendered MVC only
- **No file export or reporting**
- **Role IDs are manual** — `Role.Id` uses `ValueGeneratedNever`; you assign IDs when creating roles

## License

This project is for educational purposes (CST8256 — Web Programming Languages I).

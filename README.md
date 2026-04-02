[README.md](https://github.com/user-attachments/files/26449302/README.md)
# 📋 SurveyBasket API

A robust RESTful API built with **ASP.NET Core** for managing surveys, questions, answers, and votes — with full authentication, authorization, and production-ready features.

---

## 🚀 Features

- 🔐 **JWT Authentication & Authorization** — Secure token-based auth with refresh token support
- 📧 **Email Confirmation** — Account verification via email on registration
- 🛡️ **Role-Based Access Control (RBAC)** — Fine-grained permissions using ASP.NET Core Identity roles & claims
- ⚡ **Hybrid Caching** — In-memory + distributed caching for optimal performance
- 🚦 **Rate Limiting** — Protect endpoints from abuse with configurable rate limiting policies
- ⏱️ **Background Jobs** — Async task processing powered by Hangfire
- 🔢 **API Versioning** — Clean versioned endpoints to support future evolution
- 🌐 **Global Error Handling** — Consistent, structured error responses across the entire API
- ❤️ **Health Checks** — Built-in endpoint to monitor application and dependency health
- 🗑️ **Soft Delete** — Data is never permanently removed; safe logical deletion

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Mapping | Mapster |
| Validation | FluentValidation |
| Background Jobs | Hangfire |
| Logging | Serilog |
| Email | MailKit |

---

## 📁 Project Structure

```
SurveyBasket/
├── SurveyBasket.Api/
│   ├── Controllers/         # API endpoints
│   ├── Abstractions/        # Constants, interfaces, helpers
│   ├── Contracts/           # Request & Response DTOs
│   ├── Errors/              # Custom error types
│   └── Extensions/          # Service registration extensions
├── SurveyBasket.Application/
│   ├── Services/            # Business logic
│   └── Common/              # Shared utilities
└── SurveyBasket.Infrastructure/
    ├── Data/                # DbContext, Migrations, Seeding
    ├── Repositories/        # Data access layer
    └── Services/            # Infrastructure services (Email, Caching...)
```

---

## 📌 API Endpoints Overview

### 🔑 Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Login and get JWT token |
| `POST` | `/api/auth/refresh-token` | Get a new access token |
| `PUT` | `/api/auth/revoke-refresh-token` | Revoke a refresh token |
| `GET` | `/api/auth/confirm-email` | Confirm user email |
| `POST` | `/api/auth/resend-confirmation-email` | Resend confirmation email |
| `POST` | `/api/auth/forget-password` | Request password reset |
| `POST` | `/api/auth/reset-password` | Reset user password |

### 📋 Surveys
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/surveys` | Get all surveys |
| `GET` | `/api/surveys/{id}` | Get survey by ID |
| `POST` | `/api/surveys` | Create a new survey |
| `PUT` | `/api/surveys/{id}` | Update a survey |
| `DELETE` | `/api/surveys/{id}` | Delete a survey (soft) |
| `PUT` | `/api/surveys/{id}/togglePublish` | Publish or unpublish a survey |

### ❓ Questions
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/surveys/{surveyId}/questions` | Get all questions for a survey |
| `GET` | `/api/surveys/{surveyId}/questions/{id}` | Get a specific question |
| `POST` | `/api/surveys/{surveyId}/questions` | Add a question |
| `PUT` | `/api/surveys/{surveyId}/questions/{id}` | Update a question |
| `DELETE` | `/api/surveys/{surveyId}/questions/{id}` | Delete a question |

### 🗳️ Votes
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/surveys/{surveyId}/votes` | Submit a vote for a survey |

### 📊 Results
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/surveys/{surveyId}/results` | Get aggregated survey results |

### 👥 Users (Admin)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/users` | Get all users |
| `GET` | `/api/users/{id}` | Get user by ID |
| `POST` | `/api/users` | Create a user |
| `PUT` | `/api/users/{id}` | Update a user |
| `DELETE` | `/api/users/{id}` | Toggle user activation |

### 🏥 Health
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/health` | Application health status |

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- SMTP server (or use [Mailtrap](https://mailtrap.io/) for development)

### 1. Clone the repository

```bash
git clone https://github.com/MahmoudKhaled25/SurveyBasket.git
cd SurveyBasket
```

### 2. Configure `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SurveyBasket;Trusted_Connection=True;"
  },
  "JWT": {
    "Key": "your-secret-key-here",
    "Issuer": "SurveyBasketApp",
    "Audience": "SurveyBasketUsers",
    "ExpiryMinutes": 30
  },
  "MailSettings": {
    "Host": "smtp.example.com",
    "Port": 587,
    "UserName": "your-email@example.com",
    "Password": "your-password",
    "DisplayName": "SurveyBasket",
    "From": "your-email@example.com"
  }
}
```

### 3. Apply migrations

```bash
cd SurveyBasket.Api
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

The API will be available at `https://localhost:7xxx` and Swagger UI at `/swagger`.

---

## 🔒 Authentication

This API uses **JWT Bearer tokens**.

1. Register a new account via `POST /api/auth/register`
2. Confirm your email
3. Login via `POST /api/auth/login` to receive an `accessToken` and `refreshToken`
4. Include the token in all subsequent requests:

```
Authorization: Bearer <your-access-token>
```

---

## 👮 Roles & Permissions

| Role | Access |
|------|--------|
| `Admin` | Full access — manage users, surveys, view results |
| `Member` | Can vote on published surveys |

---

## 📦 Key NuGet Packages

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Mapster" />
<PackageReference Include="FluentValidation.AspNetCore" />
<PackageReference Include="Hangfire.AspNetCore" />
<PackageReference Include="Serilog.AspNetCore" />
<PackageReference Include="MailKit" />
```

---

## 📄 License

This project is for educational purposes.

---

> Built with ❤️ as a learning project to consolidate ASP.NET Core REST API concepts.

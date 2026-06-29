# Todo App
![Build](https://github.com/hkaab/TodoApp/actions/workflows/ci.yml/badge.svg)
![Build](https://github.com/hkaab/TodoApp/actions/workflows/ut.yml/badge.svg)
![codecov](https://codecov.io/gh/hkaab/TodoApp/branch/main/graph/badge.svg)

- **Backend:** ASP.NET Core Web API, Clean Architecture, SOLID, CQRS with MediatR, FluentValidation, global exception middleware, Serilog, Swagger, in-memory repository.

- **Frontend:** Angular standalone app, Reactive Forms, HttpClient, typed service layer, interceptor-based API error handling.

## Prerequisites

Before running the application, ensure the following software is installed on your machine.

### Backend

- .NET 8 SDK
- Visual Studio 2022 (17.14 or later) or Visual Studio Code
- Git

Verify your installation:

```bash
dotnet --version
```

### Frontend

- Node.js 22 LTS (or later)
- npm 10+ (included with Node.js)
- Angular CLI 20

Install the Angular CLI globally:

```bash
npm install -g @angular/cli
```

Verify the installation:

```bash
node --version
npm --version
ng version
```

### Recommended IDEs

- Visual Studio 2022
- Visual Studio Code

Recommended VS Code extensions:

- Angular Language Service
- C#
- ESLint
- Prettier
- GitLens

### Optional tools

- Postman or another API testing tool
- Docker Desktop (if running the application with Docker)

## Run backend

```bash
cd backend/src/TodoApp.Api
dotnet restore
dotnet run
```

API: `http://localhost:5044` or `https://localhost:5043`
Swagger: `/swagger`

## Run frontend

```bash
cd frontend
npm install
npm start
```

Angular app: `http://localhost:4200`

## Run backend & Frontend using Docker

```bash
docker compose up
```

API: `http://localhost:5044`

Swagger: `http://localhost:5044/swagger`

Angular app: `http://localhost:4200/`

## Design Decisions

### Clean Architecture

The solution is structured using Clean Architecture to separate concerns and keep the application maintainable, testable, and easy to extend.

The main layers are:

- **Domain**: Contains core business entities and rules.
- **Application**: Contains use cases, commands, queries, DTOs, validation, and interfaces.
- **Infrastructure**: Contains external implementations such as repositories and persistence.
- **API**: Exposes HTTP endpoints and handles request/response concerns.

This separation ensures the business logic is not tightly coupled to frameworks, databases, or UI concerns.

### SOLID Principles

The solution follows SOLID principles:

- **Single Responsibility**: Each class has one clear purpose.
- **Open/Closed**: New behavior can be added without modifying existing business logic.
- **Liskov Substitution**: Interfaces allow implementations to be replaced safely.
- **Interface Segregation**: Small focused interfaces are used.
- **Dependency Inversion**: High-level modules depend on abstractions instead of concrete implementations.

### CQRS Pattern

Commands and queries are separated to make the application flow clearer:

- Commands handle write operations such as creating, updating, completing, and deleting TODO items.
- Queries handle read operations such as retrieving TODO items.

This improves readability, testability, and future scalability.

### Repository Pattern

A repository abstraction is used to isolate data access from application logic.

Although the current implementation uses in-memory storage, the repository can be replaced later with Entity Framework Core, Dapper, or another persistence technology without changing the application layer.

### In-Memory Storage

The backend uses in-memory storage to keep the solution lightweight and easy to run for demonstration and assessment purposes.

This avoids database setup while still preserving a clean separation between application logic and persistence.

### Validation

Input validation is handled in the application layer using FluentValidation.

This keeps validation rules close to the use cases and ensures controllers remain thin and focused only on HTTP concerns.

### Thin Controllers

Controllers delegate business operations to commands and queries through MediatR.

This avoids placing business logic inside controllers and makes the API easier to test and maintain.

### Global Exception Handling

A global exception-handling middleware is used to centralize error handling.

This prevents repetitive `try/catch` blocks in controllers and ensures consistent API error responses.

### Angular Feature-Based Structure

The Angular frontend is organized by feature rather than technical type.

This makes the project easier to navigate and scale as more features are added.

### Reactive Forms

Reactive Forms are used for TODO creation and editing because they provide strong validation support, predictable form state management, and better testability.

### HTTP Service Layer

Angular components do not call HTTP APIs directly.

Instead, API communication is isolated in services, keeping components focused on presentation and user interaction.

### Testability

The architecture supports unit testing by depending on interfaces and separating business logic from infrastructure and framework-specific concerns.

Application handlers, validators, repositories, and controllers can be tested independently.

---

## Future Improvements

- Entity Framework Core
- SQL Server
- Authentication & Authorization
- Docker
- CI/CD
- Pagination
- Search & Filtering
- Dark Mode

---
## License

MIT


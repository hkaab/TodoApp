# Todo App
![Build](https://github.com/hkaab/TodoApp/actions/workflows/ci.yml/badge.svg)
![Build](https://github.com/hkaab/TodoApp/actions/workflows/ut.yml/badge.svg)
![codecov](https://codecov.io/gh/hkaab/TodoApp/branch/main/graph/badge.svg)

- **Backend:** ASP.NET Core Web API, Clean Architecture, SOLID, CQRS with MediatR, FluentValidation, global exception middleware, Serilog, Swagger, in-memory repository.

- **Frontend:** Angular standalone app, Reactive Forms, HttpClient, typed service layer, interceptor-based API error handling.

## Prerequisites

Before running the application, ensure the following software is installed on your machine.

### Backend

- .NET 9 SDK
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

### Optional

- Postman or another API testing tool
- Docker Desktop (if running the application with Docker)

## Run backend

```bash
cd backend/src/TodoApp.Api
dotnet restore
dotnet run
```

API: `https://localhost:7043` or `http://localhost:5043`
Swagger: `/swagger`

## Run frontend

```bash
cd frontend/todo-app-ui
npm install
npm start
```

Angular app: `http://localhost:4200`

## Notes

The backend uses an in-memory repository, as requested. It is thread-safe and scoped behind an interface so it can be replaced with EF Core, Dapper, Cosmos DB, etc. without changing the API or application layer.

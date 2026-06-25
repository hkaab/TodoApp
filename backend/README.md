# TodoApp Backend

## Architecture

```text
TodoApp.Api             HTTP, Swagger, middleware, controllers
TodoApp.Application     CQRS commands/queries, DTOs, validation, abstractions
TodoApp.Domain          Entities and domain rules
TodoApp.Infrastructure  Repository implementation and DI
```

## Run

```bash
cd src/TodoApp.Api
dotnet restore
dotnet run
```

## Test

```bash
cd backend
dotnet test
```

## API endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/v1/todos` | Get all todos |
| GET | `/api/v1/todos/{id}` | Get todo by id |
| POST | `/api/v1/todos` | Create todo |
| PUT | `/api/v1/todos/{id}` | Update title |
| PATCH | `/api/v1/todos/{id}/toggle` | Toggle completed |
| DELETE | `/api/v1/todos/{id}` | Delete todo |

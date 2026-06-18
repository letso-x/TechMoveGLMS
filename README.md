# TechMoveGLMS

TechMoveGLMS is an ASP.NET Core logistics management system for managing clients, contracts, and service requests through an MVC web interface backed by a secured Web API.

## Tech Stack

- .NET 10
- ASP.NET Core MVC with Razor views
- ASP.NET Core Web API
- Entity Framework Core with SQL Server
- JWT bearer authentication for API endpoints
- Swagger / OpenAPI via Swashbuckle
- Docker and Docker Compose
- Bootstrap, jQuery, jQuery Validation, and unobtrusive validation
- xUnit, Moq, ASP.NET Core MVC Testing, and coverlet
- GitHub Actions for automated test runs

## Architecture Overview

The solution contains three projects:

- `TechMoveGLMS.csproj` - the MVC web application.
- `TechMoveGLMS.API/TechMoveGLMS.API.csproj` - the backend Web API.
- `TechMoveGLMS.Test/TechMoveGLMS.Test.csproj` - unit and integration tests.

The MVC application uses controllers and Razor views for the user interface. MVC controllers depend on service interfaces such as `IClientService`, `IContractService`, `IServiceRequestService`, and `ICurrencyService`. These services are registered with dependency injection in `Program.cs`.

The MVC service layer communicates with the backend API through `HttpClient`. A named client, `GLMSAPI`, is configured from `GLMSAPI:BaseUrl` and defaults to `http://localhost:5001/`. `ApiService` attaches the stored JWT bearer token to outgoing API requests, while `TokenService` stores the token in session state after login.

The API project exposes REST endpoints for authentication, clients, contracts, and service requests. API controllers use Entity Framework Core directly through `AppDbContext`; no repository layer is present in the codebase. The API configures JWT bearer authentication, Swagger, JSON enum serialization, and SQL Server persistence.

The data model is centered on:

- `Client`
- `Contract`
- `ServiceRequest`

`AppDbContext` defines relationships between clients and contracts, and between contracts and service requests. The API enforces that service requests can only be created for active contracts.

The project also includes a small contract state abstraction with `IContractState`, concrete state classes, and `ContractStateFactory`. Currency conversion is implemented through `CurrencyService`, which calls the Frankfurter API to convert USD values to ZAR.

## Running Locally

### Option 1: Docker Compose

Docker Compose starts SQL Server, the backend API, and the MVC frontend.

```bash
docker compose up --build
```

Services defined in `docker-compose.yml`:

- SQL Server: `localhost:1433`
- Backend API: `http://localhost:5001`
- MVC frontend: `http://localhost:5000`

The compose file provides the API connection string and JWT key through environment variables. The MVC frontend is configured to call the API container at `http://glms-backend-api:8080/`.

### Option 2: Run With the .NET CLI

Restore and build the solution:

```bash
dotnet restore TechMoveGLMS.slnx
dotnet build TechMoveGLMS.slnx
```

Start the API:

```bash
dotnet run --project TechMoveGLMS.API/TechMoveGLMS.API.csproj
```

Start the MVC application in a second terminal:

```bash
dotnet run --project TechMoveGLMS.csproj
```

The default MVC configuration expects the API at `http://localhost:5001/`. The default application connection string uses SQL Server LocalDB:

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GLMSDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"
```

The API requires `Jwt:Key` configuration. Docker Compose provides this value for containerized runs.

The seeded login credentials are hardcoded in the API authentication controller:

```text
Username: admin
Password: password123
```

## Running Tests

A test project exists at `TechMoveGLMS.Test/TechMoveGLMS.Test.csproj`.

Run the test suite with:

```bash
dotnet test TechMoveGLMS.Test/TechMoveGLMS.Test.csproj
```

The tests include:

- Unit-style tests for currency calculation behavior.
- File validation tests for PDF extension checks.
- API integration tests using `Microsoft.AspNetCore.Mvc.Testing`.
- Authenticated integration tests for clients, contracts, and service requests.

## CI/CD

The repository includes a GitHub Actions workflow at `.github/workflows/tests.yml`.

The workflow runs on pushes and pull requests targeting `main` or `master`. It:

1. Checks out the repository.
2. Installs .NET `10.x`.
3. Restores dependencies for the test project.
4. Builds the test project.
5. Runs `dotnet test` for `TechMoveGLMS.Test/TechMoveGLMS.Test.csproj`.

No deployment workflow was found.

## Screenshots

Add screenshots of the application here.

| View | Screenshot |
| --- | --- |
| Login | _Placeholder_ |
| Clients | _Placeholder_ |
| Contracts | _Placeholder_ |
| Service Requests | _Placeholder_ |

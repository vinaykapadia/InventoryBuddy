# InventoryBuddy Copilot Instructions

## Project
- `InventoryBuddyService` is an ASP.NET Core MVC application (not Razor Pages): `Microsoft.NET.Sdk.Web`, `TargetFramework` is exactly `net10.0`.
- `InventoryBuddyDatabase` is a class library targeting exactly `net10.0`.
- `InventoryBuddyClient` is a native C++ project (`.vcxproj`), not a .NET project.
- Nullable reference types are disabled in the service and enabled in the database library; implicit usings are enabled.

## Testing
- No test project or test framework is currently configured. There is no xUnit, NUnit, or MSTest dependency. Do not assume tests exist; add a dedicated test project before adding tests.

## Dependency injection and data access
- Register application services in `InventoryBuddyService/Program.cs` using the built-in ASP.NET Core container.
- `DatabaseAccess` is registered as scoped: `AddScoped<DatabaseAccess, DatabaseAccess>()`.
- `InventoryContext` is registered with `AddDbContext`, uses SQL Server, and reads `Database:ConnectionString` from configuration.
- Use constructor injection. The current `DatabaseAccess` uses a primary constructor receiving `InventoryContext`.
- Keep EF Core entities and `InventoryContext` in `InventoryBuddyDatabase`; keep application/service data-access orchestration in `InventoryBuddyService`.

## Folder conventions
- `InventoryBuddyService/Controllers`: MVC controllers.
- `InventoryBuddyService/Views/{ControllerName}`: controller views (`.cshtml`).
- `InventoryBuddyService/Models`: service/view models.
- `InventoryBuddyDatabase/Models`: EF Core entity models.
- `InventoryBuddyDatabase/InventoryContext.cs`: EF Core context.
- `Program.cs`: service registration and HTTP pipeline.
- Configuration belongs in `appsettings.json` and `appsettings.Development.json`; do not commit secrets.

## Build and test commands
Run from the repository root in PowerShell:

```powershell
dotnet restore .\InventoryBuddy.sln
dotnet build .\InventoryBuddyService\InventoryBuddyService.csproj --configuration Release
dotnet build .\InventoryBuddyDatabase\InventoryBuddyDatabase.csproj --configuration Release
```

There are no tests to execute currently. When a test project is added, run it explicitly, for example:

```powershell
dotnet test .\<TestProject>\<TestProject>.csproj --configuration Release
```

The service uses conventional MVC routing (`MapControllers` and `MapControllerRoute`); preserve that pattern unless the application is intentionally migrated to Razor Pages.

## Legacy & Existing Code Rules
- Match the style and patterns of the surrounding file.
- Do NOT refactor unrelated code, rename existing public APIs, or upgrade NuGet packages unless explicitly requested in the issue.

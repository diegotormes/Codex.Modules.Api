# Eternet API Template

This repository includes a simple `dotnet new` template for creating new API services.

## Install template

```bash
# from the repository root
dotnet new install ./templates
```

## Create a service

```bash
# generates a project in the folder My.Service
dotnet new eternet-api -n My.Service
```

Then build the project:

```bash
cd My.Service
dotnet build -c Release
```

The generated project references `Eternet.Api.Common` and exposes a minimal `Program.cs` with extensions in `ProgramExtensions.cs`.
=======
# Codex Modules API

This repository hosts several Eternet API modules. Tests share utilities provided by the `Eternet.Testing` library placed under `tests/`.

## Reusing test utilities

When creating a new module with its own test project:

1. Add a project reference to `tests/Eternet.Testing/Eternet.Testing.csproj`.
2. Import the global usings with `global using Eternet.Testing.Assertions;` or other namespaces as needed.
3. Use `ApiWebApplicationFactory<TProgram>` to bootstrap a test host.
4. Use `FixtureFactory.Create()` to get a preconfigured AutoFixture instance.
5. Call `result.ShouldBeSuccess()` to verify domain results.

These helpers avoid duplicating setup code across modules and keep tests consistent.
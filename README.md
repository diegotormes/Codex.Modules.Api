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

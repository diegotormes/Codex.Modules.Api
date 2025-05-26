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

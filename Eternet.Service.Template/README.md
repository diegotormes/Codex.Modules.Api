# Eternet Web API Service Template

This package contains a project template for creating new Eternet microservices.
Install the template globally and use `dotnet new et-service -n MyService` to
create a service skeleton that includes:

- Serilog configuration
- Service Fabric application manifest
- Health checks
- Swashbuckle setup
- Authorization placeholders
- A test project following the CPM/MTP layout

The template uses `Eternet.Web.Infrastructure` to expose the API and follows the
structure used by existing services in this repository.

global using Mediator;

global using Eternet.Mediator;
global using Eternet.Mediator.Services;
global using Eternet.Mediator.Abstractions.Handlers;
global using Eternet.Mediator.Abstractions.Responses;

global using Microsoft.EntityFrameworkCore;

global using Eternet.Crud.Relational.Attributes;

global using Eternet.Accounting.Contracts.JournalEntries;
global using Eternet.Accounting.Api.Model;
global using Eternet.Accounting.Contracts.VatClosures.Responses;
global using Eternet.Accounting.Api.Features.VatClosures.Preview;

// ** //
global using System.Text.Json.Serialization;
global using Eternet.Accounting.Api.Configuration;
global using Eternet.Accounting.Api.Services;
global using Eternet.Accounting.Api.Swagger;
global using FirebirdSql.Data.FirebirdClient;
global using Microsoft.Extensions.Options;
global using Eternet.Mediator.Extensions;
global using Eternet.Mediator.Caching;
global using Eternet.Crud.Relational.Extensions;
global using Eternet.Crud.Relational.Services;
global using System.Reflection;
global using FluentValidation;
global using Microsoft.AspNetCore.OData;
global using Microsoft.OData.ModelBuilder;
global using Microsoft.OData.Edm;
global using Eternet.Web.Infrastructure.Extensions;
global using Eternet.Web.Infrastructure.Environment;
global using Eternet.AspNetCore.ServiceFabric;
global using Eternet.Accounting.Contracts;
global using Microsoft.AspNetCore.OData.Query.Expressions;
global using Eternet.Accounting.Contracts.JournalEntries.Responses;
global using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
global using Eternet.Api.Common.Extensions;

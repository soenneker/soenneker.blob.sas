[![](https://img.shields.io/nuget/v/Soenneker.Blob.Sas.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blob.Sas/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blob.sas/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blob.sas/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blob.Sas.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blob.Sas/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blob.sas/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blob.sas/actions/workflows/codeql.yml)

# Soenneker.Blob.Sas

A utility library for Azure Blob SAS operations For *publicly* accessible resources this util returns URLs with tokens attached to them. Typically Scoped IoC.

## Install

```bash
dotnet add package Soenneker.Blob.Sas
```

## Quick start

```csharp
using Soenneker.Blob.Sas.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddBlobSasUtilAsSingleton();
```

Registers Blob Sas Util with a singleton lifetime.

## What you get

- `IBlobSasUtil` — A utility library for Azure Blob SAS operations For *publicly* accessible resources this util returns URLs with tokens attached to them. Typically Scoped IoC.
- `BlobSasUtilRegistrar` — A utility library for Azure Blob storage sas operations.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IBlobSasUtil.GetAccountSasUri(storageUri)` | Only should be used for internal usage. | The resulting URI. |
| `BlobSasUtilRegistrar.AddBlobSasUtilAsSingleton(services)` | Registers Blob Sas Util with a singleton lifetime. | The same service collection, so additional registrations can be chained. |
| `BlobSasUtilRegistrar.AddBlobSasUtilAsScoped(services)` | Recommended. | The same service collection, so additional registrations can be chained. |

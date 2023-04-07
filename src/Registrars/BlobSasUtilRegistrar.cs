using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blob.Client.Registrars;
using Soenneker.Blob.Sas.Abstract;

namespace Soenneker.Blob.Sas.Registrars;

/// <summary>
/// A utility library for Azure Blob storage sas operations
/// </summary>
public static class BlobSasUtilRegistrar
{
    public static void AddBlobSasUtilAsSingleton(this IServiceCollection services)
    {
        services.AddBlobClientUtilAsSingleton();
        services.TryAddSingleton<IBlobSasUtil, BlobSasUtil>();
    }

    /// <summary>
    /// Recommended
    /// </summary>
    public static void AddBlobSasUtilAsScoped(this IServiceCollection services)
    {
        services.AddBlobClientUtilAsScoped();
        services.TryAddScoped<IBlobSasUtil, BlobSasUtil>();
    }
}
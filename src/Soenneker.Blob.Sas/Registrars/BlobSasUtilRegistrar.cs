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
    /// <summary>
    /// Adds blob sas util as singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
    public static IServiceCollection AddBlobSasUtilAsSingleton(this IServiceCollection services)
    {
        services.AddBlobClientUtilAsSingleton().TryAddSingleton<IBlobSasUtil, BlobSasUtil>();

        return services;
    }

    /// <summary>
    /// Recommended
    /// </summary>
    public static IServiceCollection AddBlobSasUtilAsScoped(this IServiceCollection services)
    {
        services.AddBlobClientUtilAsScoped().TryAddScoped<IBlobSasUtil, BlobSasUtil>();

        return services;
    }
}
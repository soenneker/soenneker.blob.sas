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
    /// Registers Blob Sas Util with a singleton lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddBlobSasUtilAsSingleton(this IServiceCollection services)
    {
        services.AddBlobClientUtilAsSingleton().TryAddSingleton<IBlobSasUtil, BlobSasUtil>();

        return services;
    }

    /// <summary>
    /// Recommended
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddBlobSasUtilAsScoped(this IServiceCollection services)
    {
        services.AddBlobClientUtilAsScoped().TryAddScoped<IBlobSasUtil, BlobSasUtil>();

        return services;
    }
}

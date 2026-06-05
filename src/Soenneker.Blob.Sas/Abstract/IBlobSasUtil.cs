using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blob.Sas.Abstract;

/// <summary>
/// A utility library for Azure Blob SAS operations <para/>
/// For *publicly* accessible resources this util returns URLs with tokens attached to them. <para/>
/// Typically Scoped IoC.
/// </summary>
public interface IBlobSasUtil
{
    /// <summary>
    /// Gets blob uri.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="relativeUri">The relative uri.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    string GetBlobUri(string container, string relativeUri);

    /// <summary>
    /// Gets sas uri.
    /// </summary>
    /// <param name="containerName">The container name.</param>
    /// <param name="relativeUrl">The relative url.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    string GetSasUri(string containerName, string relativeUrl);

    /// <summary>
    /// Only should be used for internal usage
    /// </summary>
    [Pure]
    Uri GetAccountSasUri(Uri storageUri);

    /// <summary>
    /// Gets sas uri with client.
    /// </summary>
    /// <param name="containerName">The container name.</param>
    /// <param name="relativeUrl">The relative url.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    [Pure]
    ValueTask<string?> GetSasUriWithClient(string containerName, string relativeUrl, CancellationToken cancellationToken = default);
}
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
    /// <param name="container">Element that will contain the rendered component.</param>
    /// <param name="relativeUri">Relative URI for the get blob uri operation.</param>
    /// <returns>The requested text.</returns>
    [Pure]
    string GetBlobUri(string container, string relativeUri);

    /// <summary>
    /// Gets sas uri.
    /// </summary>
    /// <param name="containerName">Name of the container to target.</param>
    /// <param name="relativeUrl">URL of the relative to target.</param>
    /// <returns>The requested text.</returns>
    [Pure]
    string GetSasUri(string containerName, string relativeUrl);

    /// <summary>
    /// Only should be used for internal usage
    /// </summary>
    /// <param name="storageUri">Storage URI for the get account sas uri operation.</param>
    /// <returns>The resulting URI.</returns>
    [Pure]
    Uri GetAccountSasUri(Uri storageUri);

    /// <summary>
    /// Gets sas uri with client.
    /// </summary>
    /// <param name="containerName">Name of the container to target.</param>
    /// <param name="relativeUrl">URL of the relative to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by get Sas URI With Client.</returns>
    [Pure]
    ValueTask<string?> GetSasUriWithClient(string containerName, string relativeUrl, CancellationToken cancellationToken = default);
}

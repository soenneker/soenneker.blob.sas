using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blob.Sas.Abstract;

/// <summary>
/// Builds Azure Blob Storage URLs and read-only shared access signatures.
/// </summary>
public interface IBlobSasUtil
{
    /// <summary>
    /// Builds the unsigned URL for a blob.
    /// </summary>
    /// <param name="container">Name of the blob container.</param>
    /// <param name="relativeUri">Path of the blob within the container.</param>
    /// <returns>The absolute blob URL.</returns>
    string GetBlobUri(string container, string relativeUri);

    /// <summary>
    /// Builds a read-only service SAS URL for a blob using the configured account key.
    /// </summary>
    /// <param name="containerName">Name of the blob container.</param>
    /// <param name="relativeUrl">Path of the blob within the container.</param>
    /// <returns>A signed blob URL that expires after one month.</returns>
    string GetSasUri(string containerName, string relativeUrl);

    /// <summary>
    /// Adds a one-hour, read/list account SAS to a storage service URI.
    /// </summary>
    /// <param name="storageUri">Azure Blob Storage service URI to sign.</param>
    /// <returns>The service URI with an account SAS query string.</returns>
    Uri GetAccountSasUri(Uri storageUri);

    /// <summary>
    /// Builds a read-only service SAS URL through the configured blob client.
    /// </summary>
    /// <param name="containerName">Name of the blob container.</param>
    /// <param name="relativeUrl">Path of the blob within the container.</param>
    /// <param name="cancellationToken">Token used to cancel client creation.</param>
    /// <returns>A signed blob URL, or <see langword="null"/> when the client cannot generate a SAS.</returns>
    ValueTask<string?> GetSasUriWithClient(string containerName, string relativeUrl, CancellationToken cancellationToken = default);
}

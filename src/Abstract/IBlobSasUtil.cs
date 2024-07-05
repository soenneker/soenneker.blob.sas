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
    [Pure]
    string GetSasUri(string containerName, string relativeUrl);

    /// <summary>
    /// Only should be used for internal usage
    /// </summary>
    [Pure]
    Uri GetAccountSasUri(Uri storageUri);

    [Pure]
    ValueTask<string?> GetSasUriWithClient(string containerName, string relativeUrl, CancellationToken cancellationToken = default);
}
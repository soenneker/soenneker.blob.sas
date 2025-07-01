using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Blob.Client.Abstract;
using Soenneker.Blob.Sas.Abstract;
using Soenneker.Enums.DeployEnvironment;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.DateTime;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Blob.Sas;

///<inheritdoc cref="IBlobSasUtil"/>
public sealed class BlobSasUtil : IBlobSasUtil
{
    private readonly IBlobClientUtil _clientUtil;
    private readonly ILogger<BlobSasUtil> _logger;

    private readonly string _environment;
    private readonly string _accountName;
    private readonly string _accountKey;

    private readonly Lazy<StorageSharedKeyCredential> _credential;

    public BlobSasUtil(IConfiguration config, IBlobClientUtil clientUtil, ILogger<BlobSasUtil> logger)
    {
        _clientUtil = clientUtil;
        _logger = logger;
        _environment = config.GetValueStrict<string>("Environment");
        _accountName = config.GetValueStrict<string>("Azure:Storage:Blob:AccountName");
        _accountKey = config.GetValueStrict<string>("Azure:Storage:Blob:AccountKey");

        _credential = new Lazy<StorageSharedKeyCredential>(() => new StorageSharedKeyCredential(_accountName, _accountKey), true);
    }

    public string GetSasUri(string containerName, string relativeUrl)
    {
        BlobSasBuilder sas = GetBlobBuilder(containerName, relativeUrl);

        string baseUrl = GetBlobUri(containerName, relativeUrl);

        var sasUri = new UriBuilder(baseUrl)
        {
            Query = sas.ToSasQueryParameters(_credential.Value).ToString()
        };

        return sasUri.ToString();
    }

    public string GetBlobUri(string container, string relativeUri)
    {
        string storageUri;

        if (_environment == DeployEnvironment.Local.Name)
            storageUri = "http://127.0.0.1:10000/devstoreaccount1/";
        else
            storageUri = $"https://{_accountName}.blob.core.windows.net/";

        return $"{storageUri}{container}/{relativeUri}";
    }

    public async ValueTask<string?> GetSasUriWithClient(string containerName, string relativeUrl, CancellationToken cancellationToken = default)
    {
        BlobClient client = await _clientUtil.Get(containerName, relativeUrl, cancellationToken: cancellationToken).NoSync();

        if (client.CanGenerateSasUri)
        {
            BlobSasBuilder sasBuilder = GetBlobBuilder(containerName, relativeUrl);

            Uri sasUri = client.GenerateSasUri(sasBuilder);

            var result = sasUri.ToString();

            _logger.LogDebug("SAS URI for blob is: {uri}", result);

            return result;
        }

        _logger.LogError("BlobContainerClient must be authorized with Shared Key credentials to create a service SAS.");

        return null;
    }

    private BlobSasBuilder GetBlobBuilder(string containerName, string relativeUrl)
    {
        DateTime utcNow = DateTime.UtcNow;

        var startsOn = utcNow.AddMinutes(-5).ToDateTimeOffset();
        var expiresOn = utcNow.AddMonths(1).ToDateTimeOffset();

        var sas = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = relativeUrl,
            StartsOn = startsOn,
            ExpiresOn = expiresOn,
            Resource = "b" //b = blob, c = container,
        };

        if (_environment != DeployEnvironment.Local.Name)
            sas.Protocol = SasProtocol.Https;

        sas.SetPermissions(BlobSasPermissions.Read);

        return sas;
    }

    public Uri GetAccountSasUri(Uri storageUri)
    {
        DateTime utcNow = DateTime.UtcNow;

        var startsOn = utcNow.AddMinutes(-5).ToDateTimeOffset();
        var expiresOn = utcNow.AddHours(1).ToDateTimeOffset();

        var sas = new AccountSasBuilder
        {
            StartsOn = startsOn,
            ExpiresOn = expiresOn, // Access expires in 1 hour! May want to change this
            ResourceTypes = AccountSasResourceTypes.All,
            Protocol = SasProtocol.Https,
            Services = AccountSasServices.Blobs
        };

        sas.SetPermissions(AccountSasPermissions.All);

        var credential = new StorageSharedKeyCredential(_accountName, _accountKey);

        var sasUri = new UriBuilder(storageUri.AbsoluteUri.TrimEnd('/'))
        {
            Query = sas.ToSasQueryParameters(credential).ToString()
        };

        return sasUri.Uri;
    }
}
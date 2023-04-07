using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Blob.Client.Abstract;
using Soenneker.Blob.Sas.Abstract;
using Soenneker.Enums.DeployEnvironment;

namespace Soenneker.Blob.Sas;

///<inheritdoc cref="IBlobSasUtil"/>
public class BlobSasUtil : IBlobSasUtil
{
    private readonly IBlobClientUtil _clientUtil;
    private readonly ILogger<BlobSasUtil> _logger;

    private readonly string _environment;
    private readonly string _accountName;
    private readonly string _accountKey;

    public BlobSasUtil(IConfiguration config, IBlobClientUtil clientUtil, ILogger<BlobSasUtil> logger)
    {
        IConfiguration config1 = config;
        _clientUtil = clientUtil;
        _logger = logger;
        _environment = config.GetValue<string>("Environment")!;
        _accountName = config1.GetValue<string>("Azure:Storage:Blob:AccountName")!;
        _accountKey = config1.GetValue<string>("Azure:Storage:Blob:AccountKey")!;
    }

    public string GetSasUri(string containerName, string relativeUrl)
    {
        BlobSasBuilder sas = GetBlobBuilder(containerName, relativeUrl);

        var credential = new StorageSharedKeyCredential(_accountName, _accountKey);

        string baseUrl = GetBlobUri(containerName, relativeUrl);

        var sasUri = new UriBuilder(baseUrl)
        {
            Query = sas.ToSasQueryParameters(credential).ToString()
        };

        var uri = sasUri.ToString();
        return uri;
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

    public async ValueTask<string?> GetSasUriWithClient(string containerName, string relativeUrl)
    {
        BlobClient client = await _clientUtil.GetClient(containerName, relativeUrl);

        if (client.CanGenerateSasUri)
        {
            BlobSasBuilder sasBuilder = GetBlobBuilder(containerName, relativeUrl);

            Uri sasUri = client.GenerateSasUri(sasBuilder);

            var result = sasUri.ToString();

            _logger.LogDebug("SAS URI for blob is: {uri}", result);

            return result;
        }

        _logger.LogError(@"BlobContainerClient must be authorized with Shared Key credentials to create a service SAS.");

        return null;
    }

    private BlobSasBuilder GetBlobBuilder(string containerName, string relativeUrl)
    {
        DateTime utcNow = DateTime.UtcNow;

        DateTimeOffset expiresOn = ToDateTimeOffset(utcNow.AddMonths(1));
        DateTimeOffset startsOn = ToDateTimeOffset(utcNow.AddMinutes(-5));

        var sas = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = relativeUrl,
            ExpiresOn = expiresOn,
            StartsOn = startsOn,
            Resource = "b" //b = blob, c = container,
        };

        if (_environment != DeployEnvironment.Local.Name)
            sas.Protocol = SasProtocol.Https;

        sas.SetPermissions(BlobSasPermissions.Read);

        return sas;
    }

    [Pure]
    public static DateTimeOffset ToDateTimeOffset(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime);
    }

    public Uri GetAccountSasUri(Uri storageUri)
    {
        var sas = new AccountSasBuilder
        {
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1), // Access expires in 1 hour! May want to change this
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
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
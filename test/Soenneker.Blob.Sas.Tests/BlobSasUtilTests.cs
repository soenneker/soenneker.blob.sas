using Soenneker.Blob.Sas.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blob.Sas.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class BlobSasUtilTests : HostedUnitTest
{
    private readonly IBlobSasUtil _util;

    public BlobSasUtilTests(Host host) : base(host)
    {
        _util = Resolve<IBlobSasUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}

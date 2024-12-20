using Soenneker.Blob.Sas.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Blob.Sas.Tests;

[Collection("Collection")]
public class BlobSasUtilTests : FixturedUnitTest
{
    private readonly IBlobSasUtil _util;

    public BlobSasUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IBlobSasUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}

using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints.Tests;

[Collection("Collection")]
public sealed class OriginCertsThumbprintsRunnerTests : FixturedUnitTest
{
    public OriginCertsThumbprintsRunnerTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}

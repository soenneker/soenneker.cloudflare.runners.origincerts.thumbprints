using Soenneker.Tests.HostedUnit;

namespace Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class OriginCertsThumbprintsRunnerTests : HostedUnitTest
{
    public OriginCertsThumbprintsRunnerTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}

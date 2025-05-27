using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Eternet.Accounting.Api.Tests.Fixtures;

public static class FixtureFactory
{
    public static Fixture Create()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        return fixture;
    }
}

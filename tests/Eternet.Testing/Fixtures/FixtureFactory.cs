using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Eternet.Testing.Fixtures;

public static class FixtureFactory
{
    public static Fixture Create()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        return fixture;
    }
}

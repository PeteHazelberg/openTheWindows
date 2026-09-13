using OpenTheWindows.Core.Weather;

namespace OpenTheWindows.Core.Tests.TUnit.Infrastructure;

public class GeoMathTests
{
    [Test]
    public async Task HaversineDistance_WhenCoordinatesAreIdentical_ReturnsZero()
    {
        var distance = GeoMath.HaversineDistanceMiles(40.7128, -74.0060, 40.7128, -74.0060);

        await Assert.That(distance).IsEqualTo(0d);
    }

    [Test]
    public async Task HaversineDistance_IsSymmetric()
    {
        var nycToChicago = GeoMath.HaversineDistanceMiles(40.7128, -74.0060, 41.8781, -87.6298);
        var chicagoToNyc = GeoMath.HaversineDistanceMiles(41.8781, -87.6298, 40.7128, -74.0060);

        await Assert.That(Math.Abs(nycToChicago - chicagoToNyc)).IsLessThanOrEqualTo(0.0000001d);
    }

    [Test]
    public async Task HaversineDistance_ForKnownPair_IsApproximatelyExpected()
    {
        var distance = GeoMath.HaversineDistanceMiles(40.7128, -74.0060, 41.8781, -87.6298);

        await Assert.That(distance).IsGreaterThan(600d);
        await Assert.That(distance).IsLessThan(1000d);
    }
}

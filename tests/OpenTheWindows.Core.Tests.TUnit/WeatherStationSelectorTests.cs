using OpenTheWindows.Core.Exercises;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Tests.TUnit;

public class WeatherStationSelectorTests
{
    private static WeatherStation Station(string id, double distanceMiles) => new()
    {
        StationId = id,
        Name = id,
        Latitude = 0,
        Longitude = 0,
        DistanceMiles = distanceMiles,
    };

    // EXAMPLE - fully written for you.
    [Test]
    public async Task SelectBestStation_WithOneCandidate_ReturnsIt()
    {
        // Arrange
        var stations = new[] { Station("KDEN", distanceMiles: 5.2) };

        // Act
        var result = WeatherStationSelector.SelectBestStation(stations);

        // Assert
        await Assert.That(result.StationId).IsEqualTo("KDEN");
    }

    // YOUR TURN: given several candidates with different DistanceMiles, in a
    // scrambled order, the closest one should be picked.
    [Test]
    [Skip("TODO: implement this test")]
    public async Task SelectBestStation_WithMultipleCandidates_ReturnsTheClosestOne()
    {
        await Task.CompletedTask;
    }

    // BONUS: decide what SelectBestStation should do with an empty list (hint:
    // await Assert.That(action).Throws<ArgumentException>()), then write a test for it.
    [Test]
    [Skip("TODO: implement this test")]
    public async Task SelectBestStation_WithNoCandidates_Throws()
    {
        await Task.CompletedTask;
    }
}

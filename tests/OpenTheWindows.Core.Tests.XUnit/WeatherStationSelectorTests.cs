using OpenTheWindows.Core.Exercises;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Tests.XUnit;

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
    [Fact]
    public void SelectBestStation_WithOneCandidate_ReturnsIt()
    {
        // Arrange
        var stations = new[] { Station("KDEN", distanceMiles: 5.2) };

        // Act
        var result = WeatherStationSelector.SelectBestStation(stations);

        // Assert
        Assert.Equal("KDEN", result.StationId);
    }

    // YOUR TURN: given several candidates with different DistanceMiles, in a
    // scrambled order, the closest one should be picked.
    [Fact(Skip = "TODO: implement this test")]
    public void SelectBestStation_WithMultipleCandidates_ReturnsTheClosestOne()
    {
    }

    // BONUS: decide what SelectBestStation should do with an empty list (hint:
    // Assert.Throws<ArgumentException>(...)), then write a test for it.
    [Fact(Skip = "TODO: implement this test")]
    public void SelectBestStation_WithNoCandidates_Throws()
    {
    }
}

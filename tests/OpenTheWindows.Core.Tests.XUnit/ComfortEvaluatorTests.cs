using OpenTheWindows.Core.Exercises;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Tests.XUnit;

public class ComfortEvaluatorTests
{
    private static ComfortPreferences DefaultPreferences() => new()
    {
        MinComfortableTemperatureF = 60,
        MaxComfortableTemperatureF = 75,
        MinComfortableDewPointF = 35,
        MaxComfortableDewPointF = 60,
    };

    private static WeatherReading Reading(float temperatureF, float dewPointF) => new()
    {
        TemperatureF = temperatureF,
        DewPointF = dewPointF,
        RelativeHumidityPercent = 50,
        StationId = "TEST",
        ObservedAtUtc = DateTimeOffset.UtcNow,
    };

    // EXAMPLE - fully written for you. This shows the Arrange/Act/Assert pattern
    // you'll reuse below: set up inputs, call the method under test, check the result.
    [Fact]
    public void ShouldOpenWindows_WhenTemperatureAndDewPointAreComfortable_ReturnsTrue()
    {
        // Arrange
        var reading = Reading(temperatureF: 68, dewPointF: 45);
        var preferences = DefaultPreferences();

        // Act
        var result = ComfortEvaluator.ShouldOpenWindows(reading, preferences);

        // Assert
        Assert.True(result);
    }

    // YOUR TURN: temperature is above MaxComfortableTemperatureF (too hot), but the
    // dew point is comfortable. Remove [Fact(Skip=...)] and write the test body.
    [Fact(Skip = "TODO: implement this test")]
    public void ShouldOpenWindows_WhenTemperatureIsTooHot_ReturnsFalse()
    {
    }

    // YOUR TURN: temperature is below MinComfortableTemperatureF (too cold), but the
    // dew point is comfortable.
    [Fact(Skip = "TODO: implement this test")]
    public void ShouldOpenWindows_WhenTemperatureIsTooCold_ReturnsFalse()
    {
    }

    // YOUR TURN: temperature is comfortable, but the dew point is above
    // MaxComfortableDewPointF (muggy/sticky air outside).
    [Fact(Skip = "TODO: implement this test")]
    public void ShouldOpenWindows_WhenDewPointIsTooHigh_ReturnsFalse()
    {
    }

    // BONUS: what should happen exactly AT a boundary, e.g. temperature equals
    // MaxComfortableTemperatureF exactly? Decide on a behavior (inclusive is
    // recommended), write a test for it, then make ComfortEvaluator match.
    [Fact(Skip = "TODO: implement this test")]
    public void ShouldOpenWindows_WhenTemperatureIsExactlyAtTheBoundary_ReturnsTrue()
    {
    }
}

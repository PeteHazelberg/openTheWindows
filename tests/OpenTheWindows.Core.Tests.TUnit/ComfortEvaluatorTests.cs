using OpenTheWindows.Core.Exercises;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Tests.TUnit;

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

    // EXAMPLE - fully written for you. Same scenario as the xUnit version, so you
    // can compare how the two frameworks read.
    [Test]
    public async Task ShouldOpenWindows_WhenTemperatureAndDewPointAreComfortable_ReturnsTrue()
    {
        // Arrange
        var reading = Reading(temperatureF: 68, dewPointF: 45);
        var preferences = DefaultPreferences();

        // Act
        var result = ComfortEvaluator.ShouldOpenWindows(reading, preferences);

        // Assert
        await Assert.That(result).IsTrue();
    }

    // YOUR TURN: temperature is above MaxComfortableTemperatureF (too hot), but the
    // dew point is comfortable. Remove [Skip] and write the test body.
    [Test]
    [Skip("TODO: implement this test")]
    public async Task ShouldOpenWindows_WhenTemperatureIsTooHot_ReturnsFalse()
    {
        await Task.CompletedTask;
    }

    // YOUR TURN: temperature is below MinComfortableTemperatureF (too cold), but the
    // dew point is comfortable.
    [Test]
    [Skip("TODO: implement this test")]
    public async Task ShouldOpenWindows_WhenTemperatureIsTooCold_ReturnsFalse()
    {
        await Task.CompletedTask;
    }

    // YOUR TURN: temperature is comfortable, but the dew point is above
    // MaxComfortableDewPointF (muggy/sticky air outside).
    [Test]
    [Skip("TODO: implement this test")]
    public async Task ShouldOpenWindows_WhenDewPointIsTooHigh_ReturnsFalse()
    {
        await Task.CompletedTask;
    }

    // BONUS: what should happen exactly AT a boundary, e.g. temperature equals
    // MaxComfortableTemperatureF exactly? Decide on a behavior (inclusive is
    // recommended), write a test for it, then make ComfortEvaluator match.
    [Test]
    [Skip("TODO: implement this test")]
    public async Task ShouldOpenWindows_WhenTemperatureIsExactlyAtTheBoundary_ReturnsTrue()
    {
        await Task.CompletedTask;
    }
}

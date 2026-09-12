using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Exercises;

/// <summary>
/// EXERCISE (for the human, not AI-assisted!): decide whether it's a good time to
/// open the windows instead of running the AC.
///
/// Requirements:
///  - Return true only when BOTH of these are true:
///      1. reading.TemperatureF is between preferences.MinComfortableTemperatureF and
///         preferences.MaxComfortableTemperatureF (inclusive).
///      2. reading.DewPointF is between preferences.MinComfortableDewPointF and
///         preferences.MaxComfortableDewPointF (inclusive).
///  - Otherwise return false.
///
/// This is the core "decision" of the whole app - the unit tests in the test
/// projects describe the exact scenarios it needs to handle. Start there!
/// </summary>
public static class ComfortEvaluator
{
    public static bool ShouldOpenWindows(WeatherReading reading, ComfortPreferences preferences)
    {
        throw new NotImplementedException("TODO: implement ComfortEvaluator.ShouldOpenWindows");
    }
}

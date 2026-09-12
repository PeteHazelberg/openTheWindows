namespace OpenTheWindows.Core.Models;

/// <summary>
/// A user's comfort settings: the outdoor temperature and dew point ranges
/// they consider comfortable enough to open windows instead of running the AC.
/// All values are in Fahrenheit.
/// </summary>
public sealed class ComfortPreferences
{
    public required float MinComfortableTemperatureF { get; init; }

    public required float MaxComfortableTemperatureF { get; init; }

    public required float MinComfortableDewPointF { get; init; }

    public required float MaxComfortableDewPointF { get; init; }
}

namespace OpenTheWindows.Core.Models;

/// <summary>
/// A single outdoor weather observation pulled from a weather station,
/// reported in Fahrenheit / percent so it lines up with <see cref="ComfortPreferences"/>.
/// </summary>
public sealed class WeatherReading
{
    public required float TemperatureF { get; init; }

    public required float DewPointF { get; init; }

    public required float RelativeHumidityPercent { get; init; }

    public required string StationId { get; init; }

    public required DateTimeOffset ObservedAtUtc { get; init; }
}

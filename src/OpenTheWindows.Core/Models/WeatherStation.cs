namespace OpenTheWindows.Core.Models;

/// <summary>
/// A candidate weather station near a <see cref="Location"/> that the National
/// Weather Service can report observations for.
/// </summary>
public sealed class WeatherStation
{
    public required string StationId { get; init; }

    public required string Name { get; init; }

    public required double Latitude { get; init; }

    public required double Longitude { get; init; }

    /// <summary>
    /// Straight-line distance from the residence to this station, in miles.
    /// Pre-computed so selection logic doesn't need to do geo-math.
    /// </summary>
    public required double DistanceMiles { get; init; }
}

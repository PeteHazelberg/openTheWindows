namespace OpenTheWindows.Core.Models;

/// <summary>
/// The residence whose outdoor conditions we care about.
/// Starts as just a US zip code; latitude/longitude are filled in later
/// by a geocoding step so we can query the National Weather Service.
/// </summary>
public sealed class Location
{
    public required string ZipCode { get; init; }

    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    public bool HasCoordinates => Latitude is not null && Longitude is not null;
}

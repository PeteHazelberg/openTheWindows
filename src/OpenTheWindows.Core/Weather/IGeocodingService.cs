using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Weather;

/// <summary>
/// Converts a US zip code into latitude/longitude. Implemented using the free,
/// keyless US Census Bureau geocoder (US-only, which matches our NWS-only scope).
/// This is plumbing/infrastructure code, not one of the "exercise" classes.
/// </summary>
public interface IGeocodingService
{
    /// <summary>Returns a new Location with Latitude/Longitude populated, or null if the zip code could not be resolved.</summary>
    Task<Location?> GeocodeAsync(string zipCode, CancellationToken cancellationToken = default);
}

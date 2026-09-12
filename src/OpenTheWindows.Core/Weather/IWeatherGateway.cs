using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Weather;

/// <summary>
/// Talks to the National Weather Service (api.weather.gov) - a free, keyless,
/// US-only weather API. This is plumbing/infrastructure code that calls the
/// network and parses JSON; it is NOT one of the "exercise" classes.
/// </summary>
public interface IWeatherGateway
{
    /// <summary>Lists the weather stations NWS considers nearest to the given location.</summary>
    Task<IReadOnlyList<WeatherStation>> GetNearbyStationsAsync(Location location, CancellationToken cancellationToken = default);

    /// <summary>Fetches the latest observation reported by a specific station.</summary>
    Task<WeatherReading> GetLatestObservationAsync(WeatherStation station, CancellationToken cancellationToken = default);
}

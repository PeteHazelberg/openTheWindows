using System.Net.Http.Json;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Weather;

/// <summary>
/// Calls the free, keyless National Weather Service API (api.weather.gov).
/// This is infrastructure/plumbing code (network + JSON parsing + unit conversion) -
/// not one of the exercises. NWS requires a descriptive User-Agent identifying the app.
/// </summary>
public sealed class NwsWeatherGateway(HttpClient httpClient) : IWeatherGateway
{
    public async Task<IReadOnlyList<WeatherStation>> GetNearbyStationsAsync(Location location, CancellationToken cancellationToken = default)
    {
        if (!location.HasCoordinates)
        {
            throw new ArgumentException("Location must be geocoded (Latitude/Longitude set) before looking up stations.", nameof(location));
        }

        var points = await httpClient.GetFromJsonAsync(
            $"https://api.weather.gov/points/{location.Latitude:F4},{location.Longitude:F4}",
            WeatherJsonContext.Default.NwsPointsResponse,
            cancellationToken) ?? throw new InvalidOperationException("NWS points lookup returned no data.");

        var stations = await httpClient.GetFromJsonAsync(
            points.Properties.ObservationStationsUrl,
            WeatherJsonContext.Default.NwsStationsResponse,
            cancellationToken) ?? throw new InvalidOperationException("NWS station lookup returned no data.");

        return stations.Features.Select(f => new WeatherStation
        {
            StationId = f.Properties.StationIdentifier,
            Name = f.Properties.Name,
            Longitude = f.Geometry.Coordinates[0],
            Latitude = f.Geometry.Coordinates[1],
            DistanceMiles = GeoMath.HaversineDistanceMiles(
                location.Latitude!.Value, location.Longitude!.Value,
                f.Geometry.Coordinates[1], f.Geometry.Coordinates[0]),
        }).ToList();
    }

    public async Task<WeatherReading> GetLatestObservationAsync(WeatherStation station, CancellationToken cancellationToken = default)
    {
        var observation = await httpClient.GetFromJsonAsync(
            $"https://api.weather.gov/stations/{station.StationId}/observations/latest",
            WeatherJsonContext.Default.NwsObservationResponse,
            cancellationToken) ?? throw new InvalidOperationException("NWS observation lookup returned no data.");

        var props = observation.Properties;
        return new WeatherReading
        {
            TemperatureF = CelsiusToFahrenheit(props.Temperature.Value),
            DewPointF = CelsiusToFahrenheit(props.DewPoint.Value),
            RelativeHumidityPercent = (float)(props.RelativeHumidity.Value ?? 0),
            StationId = station.StationId,
            ObservedAtUtc = props.Timestamp,
        };
    }

    private static float CelsiusToFahrenheit(double? celsius) => (float)((celsius ?? 0) * 9.0 / 5.0 + 32.0);
}

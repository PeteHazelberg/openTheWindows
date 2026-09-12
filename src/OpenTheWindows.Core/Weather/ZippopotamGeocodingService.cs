using System.Globalization;
using System.Net.Http.Json;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Weather;

/// <summary>
/// Resolves a US zip code to coordinates using the free, keyless zippopotam.us API.
/// This is infrastructure/plumbing code (network + JSON parsing) - not one of the exercises.
/// </summary>
public sealed class ZippopotamGeocodingService(HttpClient httpClient) : IGeocodingService
{
    public async Task<Location?> GeocodeAsync(string zipCode, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetFromJsonAsync(
            $"https://api.zippopotam.us/us/{zipCode}",
            WeatherJsonContext.Default.ZippopotamResponse,
            cancellationToken);

        var place = response?.Places.FirstOrDefault();
        if (place is null)
        {
            return null;
        }

        return new Location
        {
            ZipCode = zipCode,
            Latitude = double.Parse(place.Latitude, CultureInfo.InvariantCulture),
            Longitude = double.Parse(place.Longitude, CultureInfo.InvariantCulture),
        };
    }
}

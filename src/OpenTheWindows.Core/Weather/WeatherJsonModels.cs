using System.Text.Json.Serialization;

namespace OpenTheWindows.Core.Weather;

// DTOs for https://api.zippopotam.us/us/{zip} - a free, keyless zip-code lookup.
internal sealed class ZippopotamResponse
{
    [JsonPropertyName("places")]
    public List<ZippopotamPlace> Places { get; init; } = [];
}

internal sealed class ZippopotamPlace
{
    [JsonPropertyName("place name")]
    public string PlaceName { get; init; } = "";

    [JsonPropertyName("latitude")]
    public string Latitude { get; init; } = "";

    [JsonPropertyName("longitude")]
    public string Longitude { get; init; } = "";
}

// DTOs for the small slice of api.weather.gov's GeoJSON responses that we need.
internal sealed class NwsPointsResponse
{
    [JsonPropertyName("properties")]
    public NwsPointsProperties Properties { get; init; } = new();
}

internal sealed class NwsPointsProperties
{
    [JsonPropertyName("observationStations")]
    public string ObservationStationsUrl { get; init; } = "";
}

internal sealed class NwsStationsResponse
{
    [JsonPropertyName("features")]
    public List<NwsStationFeature> Features { get; init; } = [];
}

internal sealed class NwsStationFeature
{
    [JsonPropertyName("geometry")]
    public NwsGeometry Geometry { get; init; } = new();

    [JsonPropertyName("properties")]
    public NwsStationProperties Properties { get; init; } = new();
}

internal sealed class NwsGeometry
{
    // GeoJSON order is [longitude, latitude].
    [JsonPropertyName("coordinates")]
    public double[] Coordinates { get; init; } = [0, 0];
}

internal sealed class NwsStationProperties
{
    [JsonPropertyName("stationIdentifier")]
    public string StationIdentifier { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";
}

internal sealed class NwsObservationResponse
{
    [JsonPropertyName("properties")]
    public NwsObservationProperties Properties { get; init; } = new();
}

internal sealed class NwsObservationProperties
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("temperature")]
    public NwsQuantity Temperature { get; init; } = new();

    [JsonPropertyName("dewpoint")]
    public NwsQuantity DewPoint { get; init; } = new();

    [JsonPropertyName("relativeHumidity")]
    public NwsQuantity RelativeHumidity { get; init; } = new();
}

internal sealed class NwsQuantity
{
    // NWS reports these as degrees Celsius / percent.
    [JsonPropertyName("value")]
    public double? Value { get; init; }
}

[JsonSerializable(typeof(ZippopotamResponse))]
[JsonSerializable(typeof(NwsPointsResponse))]
[JsonSerializable(typeof(NwsStationsResponse))]
[JsonSerializable(typeof(NwsObservationResponse))]
internal sealed partial class WeatherJsonContext : JsonSerializerContext;

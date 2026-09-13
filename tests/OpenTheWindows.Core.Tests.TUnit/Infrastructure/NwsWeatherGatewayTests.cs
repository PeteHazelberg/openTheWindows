using System.Net.Http.Json;
using OpenTheWindows.Core.Models;
using OpenTheWindows.Core.Weather;

namespace OpenTheWindows.Core.Tests.TUnit.Infrastructure;

public class NwsWeatherGatewayTests
{
    [Test]
    public async Task GetNearbyStationsAsync_WhenLocationHasCoordinates_ReturnsMappedWeatherStations()
    {
        var handler = new FakeHttpMessageHandler(
            request =>
            {
                if (request.RequestUri is not null && request.RequestUri.AbsoluteUri.Contains("/points/"))
                {
                    return FakeHttpMessageHandler.JsonResponse("""
                        {
                          "properties": {
                            "observationStations": "https://api.weather.gov/stations"
                          }
                        }
                        """);
                }

                throw new InvalidOperationException($"Unexpected request: {request.RequestUri}");
            },
            _ => FakeHttpMessageHandler.JsonResponse("""
                {
                  "features": [
                    {
                      "geometry": {
                        "coordinates": [-87.6298, 41.8781]
                      },
                      "properties": {
                        "stationIdentifier": "KORD",
                        "name": "Chicago O'Hare"
                      }
                    }
                  ]
                }
                """));

        var gateway = new NwsWeatherGateway(new HttpClient(handler));
        var location = new Location
        {
            ZipCode = "60601",
            Latitude = 41.8781,
            Longitude = -87.6298,
        };

        var stations = await gateway.GetNearbyStationsAsync(location);

        await Assert.That(stations.Count).IsEqualTo(1);
        await Assert.That(stations[0].StationId).IsEqualTo("KORD");
        await Assert.That(stations[0].Name).IsEqualTo("Chicago O'Hare");
        await Assert.That(stations[0].Latitude).IsEqualTo(41.8781);
        await Assert.That(stations[0].Longitude).IsEqualTo(-87.6298);
        await Assert.That(stations[0].DistanceMiles).IsEqualTo(0d);
    }

    [Test]
    public async Task GetNearbyStationsAsync_WhenLocationLacksCoordinates_ThrowsArgumentException()
    {
        var gateway = new NwsWeatherGateway(new HttpClient(new FakeHttpMessageHandler()));
        var location = new Location
        {
            ZipCode = "60601",
            Latitude = null,
            Longitude = null,
        };

        await Assert.ThrowsAsync<ArgumentException>(async () => await gateway.GetNearbyStationsAsync(location));
    }

    [Test]
    public async Task GetLatestObservationAsync_ConvertsCelsiusToFahrenheit()
    {
        var handler = new FakeHttpMessageHandler(
            _ => FakeHttpMessageHandler.JsonResponse("""
                {
                  "properties": {
                    "timestamp": "2024-01-12T17:00:00Z",
                    "temperature": { "value": 20 },
                    "dewpoint": { "value": 10 },
                    "relativeHumidity": { "value": 60 }
                  }
                }
                """));

        var gateway = new NwsWeatherGateway(new HttpClient(handler));
        var station = new WeatherStation
        {
            StationId = "KORD",
            Name = "Chicago O'Hare",
            Latitude = 41.8781,
            Longitude = -87.6298,
            DistanceMiles = 0,
        };

        var reading = await gateway.GetLatestObservationAsync(station);

        await Assert.That(reading.TemperatureF).IsEqualTo(68f);
        await Assert.That(reading.DewPointF).IsEqualTo(50f);
        await Assert.That(reading.RelativeHumidityPercent).IsEqualTo(60f);
        await Assert.That(reading.StationId).IsEqualTo("KORD");
    }
}

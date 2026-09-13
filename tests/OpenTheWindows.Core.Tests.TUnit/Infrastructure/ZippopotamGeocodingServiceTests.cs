using OpenTheWindows.Core.Models;
using OpenTheWindows.Core.Weather;

namespace OpenTheWindows.Core.Tests.TUnit.Infrastructure;

public class ZippopotamGeocodingServiceTests
{
    [Test]
    public async Task GeocodeAsync_WhenZipCodeIsFound_ReturnsLocationWithCoordinates()
    {
        var handler = new FakeHttpMessageHandler(
            _ => FakeHttpMessageHandler.JsonResponse("""
                {
                  "places": [
                    {
                      "place name": "Chicago",
                      "latitude": "41.8781",
                      "longitude": "-87.6298"
                    }
                  ]
                }
                """));

        var geocoder = new ZippopotamGeocodingService(new HttpClient(handler));

        var result = await geocoder.GeocodeAsync("60601");

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ZipCode).IsEqualTo("60601");
        await Assert.That(result.Latitude).IsEqualTo(41.8781);
        await Assert.That(result.Longitude).IsEqualTo(-87.6298);
    }

    [Test]
    public async Task GeocodeAsync_WhenZipCodeHasNoPlaces_ReturnsNull()
    {
        var handler = new FakeHttpMessageHandler(
            _ => FakeHttpMessageHandler.JsonResponse("""
                {
                  "places": []
                }
                """));

        var geocoder = new ZippopotamGeocodingService(new HttpClient(handler));

        var result = await geocoder.GeocodeAsync("00000");

        await Assert.That(result).IsNull();
    }
}

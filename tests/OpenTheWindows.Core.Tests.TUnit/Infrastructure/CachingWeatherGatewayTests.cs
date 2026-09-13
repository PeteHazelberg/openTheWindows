using OpenTheWindows.Core.Models;
using OpenTheWindows.Core.Weather;

namespace OpenTheWindows.Core.Tests.TUnit.Infrastructure;

public class CachingWeatherGatewayTests
{
    private static WeatherReading Reading(string stationId, float temperatureF = 68, float dewPointF = 45)
        => new()
        {
            StationId = stationId,
            TemperatureF = temperatureF,
            DewPointF = dewPointF,
            RelativeHumidityPercent = 50,
            ObservedAtUtc = DateTimeOffset.UtcNow,
        };

    private static WeatherStation Station(string stationId) => new()
    {
        StationId = stationId,
        Name = stationId,
        Latitude = 41.8781,
        Longitude = -87.6298,
        DistanceMiles = 0,
    };

    [Test]
    public async Task GetLatestObservationAsync_WhenCacheMiss_ReturnsFreshReadingFromInnerGateway()
    {
        var innerGateway = new CountingWeatherGateway(Reading("KDEN", temperatureF: 70, dewPointF: 50));
        var cachePath = GetTempCachePath();
        var gateway = new CachingWeatherGateway(innerGateway, TimeSpan.FromMinutes(30), cachePath);

        var result = await gateway.GetLatestObservationAsync(Station("KDEN"));

        await Assert.That(result.TemperatureF).IsEqualTo(70f);
        await Assert.That(innerGateway.CallCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetLatestObservationAsync_WhenCacheEntryIsStillFresh_UsesCachedValue()
    {
        var innerGateway = new CountingWeatherGateway(Reading("KDEN", temperatureF: 72, dewPointF: 48));
        var cachePath = GetTempCachePath();
        var gateway = new CachingWeatherGateway(innerGateway, TimeSpan.FromMinutes(30), cachePath);
        var station = Station("KDEN");

        var firstResult = await gateway.GetLatestObservationAsync(station);
        var secondResult = await gateway.GetLatestObservationAsync(station);

        await Assert.That(firstResult.TemperatureF).IsEqualTo(72f);
        await Assert.That(secondResult.TemperatureF).IsEqualTo(72f);
        await Assert.That(innerGateway.CallCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetLatestObservationAsync_WhenCacheEntryHasExpired_RefreshesFromInnerGateway()
    {
        var innerGateway = new CountingWeatherGateway(Reading("KDEN", temperatureF: 68, dewPointF: 45));
        var cachePath = GetTempCachePath();
        var gateway = new CachingWeatherGateway(innerGateway, TimeSpan.FromMilliseconds(25), cachePath);
        var station = Station("KDEN");

        await gateway.GetLatestObservationAsync(station);
        await Task.Delay(75);
        var refreshed = await gateway.GetLatestObservationAsync(station);

        await Assert.That(refreshed.TemperatureF).IsEqualTo(68f);
        await Assert.That(innerGateway.CallCount).IsEqualTo(2);
    }

    [Test]
    public async Task GetLatestObservationAsync_WhenMultipleCallsRace_OnlyHitsInnerGatewayOnce()
    {
        var innerGateway = new CountingWeatherGateway(Reading("KDEN", temperatureF: 74, dewPointF: 52));
        var cachePath = GetTempCachePath();
        var gateway = new CachingWeatherGateway(innerGateway, TimeSpan.FromMinutes(30), cachePath);
        var station = Station("KDEN");

        var tasks = Enumerable.Range(0, 8)
            .Select(_ => gateway.GetLatestObservationAsync(station))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        await Assert.That(results.Length).IsEqualTo(8);
        await Assert.That(results.All(r => r.TemperatureF == 74f)).IsTrue();
        await Assert.That(innerGateway.CallCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetLatestObservationAsync_WhenCacheWriteFails_ReturnsFreshObservation()
    {
        var innerGateway = new CountingWeatherGateway(Reading("KDEN", temperatureF: 76, dewPointF: 58));
        var cachePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "weather-cache.json");
        var gateway = new CachingWeatherGateway(innerGateway, TimeSpan.FromMinutes(30), cachePath);
        var station = Station("KDEN");

        var result = await gateway.GetLatestObservationAsync(station);

        await Assert.That(result.TemperatureF).IsEqualTo(76f);
        await Assert.That(innerGateway.CallCount).IsEqualTo(1);
    }

    private static string GetTempCachePath()
    {
        var directory = Path.Combine(Path.GetTempPath(), "openTheWindows-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "weather-cache.json");
    }

    private sealed class CountingWeatherGateway(WeatherReading reading) : IWeatherGateway
    {
        public int CallCount { get; private set; }

        public Task<IReadOnlyList<WeatherStation>> GetNearbyStationsAsync(Location location, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<WeatherStation>>(Array.Empty<WeatherStation>());
        }

        public Task<WeatherReading> GetLatestObservationAsync(WeatherStation station, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.FromResult(reading);
        }
    }
}

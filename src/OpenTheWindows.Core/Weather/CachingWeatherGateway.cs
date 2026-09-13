using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Weather;

/// <summary>
/// Decorates an <see cref="IWeatherGateway"/> with a simple file-backed cache so the
/// app does not hammer the free weather service during development, testing, or repeated
/// app launches. The cache is intentionally explicit and small: it stores only a recent
/// observation and expires after a configurable window.
/// </summary>
public sealed class CachingWeatherGateway : IWeatherGateway
{
    private const string DefaultCacheFileName = "weather-cache.json";

    private readonly IWeatherGateway _innerGateway;
    private readonly TimeSpan _cacheDuration;
    private readonly string _cacheFilePath;
    private readonly SemaphoreSlim _cacheLock = new(1, 1);

    public CachingWeatherGateway(
        IWeatherGateway innerGateway,
        TimeSpan? cacheDuration = null,
        string? cacheFilePath = null)
    {
        _innerGateway = innerGateway ?? throw new ArgumentNullException(nameof(innerGateway));
        _cacheDuration = cacheDuration ?? TimeSpan.FromMinutes(30);
        _cacheFilePath = cacheFilePath ?? GetDefaultCacheFilePath();
    }

    public async Task<IReadOnlyList<WeatherStation>> GetNearbyStationsAsync(Location location, CancellationToken cancellationToken = default)
    {
        return await _innerGateway.GetNearbyStationsAsync(location, cancellationToken);
    }

    public async Task<WeatherReading> GetLatestObservationAsync(WeatherStation station, CancellationToken cancellationToken = default)
    {
        await _cacheLock.WaitAsync(cancellationToken);
        try
        {
            var cacheEntries = await LoadCacheAsync(cancellationToken);

            if (cacheEntries.TryGetValue(station.StationId, out var cachedReading) &&
                cachedReading.ExpiresAtUtc > DateTimeOffset.UtcNow)
            {
                return cachedReading.WeatherReading;
            }

            var freshReading = await _innerGateway.GetLatestObservationAsync(station, cancellationToken);

            cacheEntries[station.StationId] = new WeatherCacheEntry
            {
                ExpiresAtUtc = DateTimeOffset.UtcNow + _cacheDuration,
                WeatherReading = freshReading,
            };

            try
            {
                await SaveCacheAsync(cacheEntries, cancellationToken);
            }
            catch (IOException) when (!cancellationToken.IsCancellationRequested)
            {
                // Cache writes are an optimization only. A failed write should not turn
                // a successful NWS fetch into a user-visible error.
            }
            catch (UnauthorizedAccessException) when (!cancellationToken.IsCancellationRequested)
            {
                // Same idea: fail the cache quietly, keep the fresh observation.
            }

            return freshReading;
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    private static string GetDefaultCacheFilePath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var directory = Path.Combine(localAppData, "OpenTheWindows");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, DefaultCacheFileName);
    }

    private async Task<Dictionary<string, WeatherCacheEntry>> LoadCacheAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_cacheFilePath))
        {
            return new Dictionary<string, WeatherCacheEntry>(StringComparer.OrdinalIgnoreCase);
        }

        try
        {
            var json = await File.ReadAllTextAsync(_cacheFilePath, cancellationToken);
            var payload = JsonSerializer.Deserialize(json, WeatherCacheJsonContext.Default.WeatherCacheFile)
                ?? new WeatherCacheFile();

            return payload.Entries ?? new Dictionary<string, WeatherCacheEntry>(StringComparer.OrdinalIgnoreCase);
        }
        catch (JsonException)
        {
            return new Dictionary<string, WeatherCacheEntry>(StringComparer.OrdinalIgnoreCase);
        }
        catch (IOException)
        {
            return new Dictionary<string, WeatherCacheEntry>(StringComparer.OrdinalIgnoreCase);
        }
    }

    private async Task SaveCacheAsync(Dictionary<string, WeatherCacheEntry> entries, CancellationToken cancellationToken)
    {
        var payload = new WeatherCacheFile
        {
            Entries = entries,
        };

        var json = JsonSerializer.Serialize(payload, WeatherCacheJsonContext.Default.WeatherCacheFile);
        await File.WriteAllTextAsync(_cacheFilePath, json, cancellationToken);
    }

    internal sealed class WeatherCacheFile
    {
        [JsonPropertyName("entries")]
        public Dictionary<string, WeatherCacheEntry> Entries { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    internal sealed class WeatherCacheEntry
    {
        [JsonPropertyName("expiresAtUtc")]
        public DateTimeOffset ExpiresAtUtc { get; set; }

        [JsonPropertyName("weatherReading")]
        public WeatherReading WeatherReading { get; set; } = new()
        {
            TemperatureF = 0,
            DewPointF = 0,
            RelativeHumidityPercent = 0,
            StationId = string.Empty,
            ObservedAtUtc = DateTimeOffset.UnixEpoch,
        };
    }
}

[JsonSerializable(typeof(CachingWeatherGateway.WeatherCacheFile))]
internal sealed partial class WeatherCacheJsonContext : JsonSerializerContext;

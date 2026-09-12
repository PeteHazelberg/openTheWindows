using OpenTheWindows.Core.Exercises;
using OpenTheWindows.Core.Models;
using OpenTheWindows.Core.Weather;

namespace OpenTheWindows.App.Native;

public partial class MainPage : ContentPage
{
	private static readonly HttpClient HttpClient = new()
	{
		DefaultRequestHeaders = { { "User-Agent", "OpenTheWindows-Prototype (github.com/PeteHazelberg/openTheWindows)" } },
	};

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCheckClicked(object? sender, EventArgs e)
	{
		ResultLabel.Text = "Checking...";

		try
		{
			var preferences = new ComfortPreferences
			{
				MinComfortableTemperatureF = float.Parse(MinTempEntry.Text ?? "60"),
				MaxComfortableTemperatureF = float.Parse(MaxTempEntry.Text ?? "75"),
				MinComfortableDewPointF = float.Parse(MinDewPointEntry.Text ?? "35"),
				MaxComfortableDewPointF = float.Parse(MaxDewPointEntry.Text ?? "60"),
			};

			var geocoder = new ZippopotamGeocodingService(HttpClient);
			var location = await geocoder.GeocodeAsync(ZipCodeEntry.Text ?? "")
				?? throw new InvalidOperationException("Could not find that zip code.");

			var gateway = new NwsWeatherGateway(HttpClient);
			var stations = await gateway.GetNearbyStationsAsync(location);
			var station = WeatherStationSelector.SelectBestStation(stations);
			var reading = await gateway.GetLatestObservationAsync(station);

			var shouldOpen = ComfortEvaluator.ShouldOpenWindows(reading, preferences);

			ResultLabel.Text =
				$"{reading.TemperatureF:F0}\u00b0F, dew point {reading.DewPointF:F0}\u00b0F (from {station.Name}).\n" +
				(shouldOpen ? "Open the windows!" : "Keep the windows closed.");
		}
		catch (NotImplementedException)
		{
			ResultLabel.Text = "Waiting on the Exercises classes to be implemented!";
		}
		catch (Exception ex)
		{
			ResultLabel.Text = $"Something went wrong: {ex.Message}";
		}
	}
}

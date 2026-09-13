using Microsoft.Extensions.Logging;
using OpenTheWindows.Core.Weather;

namespace OpenTheWindows.App.BlazorHybrid;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		builder.Services.AddSingleton(_ =>
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("User-Agent", "OpenTheWindows-Prototype (github.com/PeteHazelberg/openTheWindows)");
			return httpClient;
		});

		builder.Services.AddSingleton<IWeatherGateway>(sp =>
		{
			var httpClient = sp.GetRequiredService<HttpClient>();
			var innerGateway = new NwsWeatherGateway(httpClient);
			return new CachingWeatherGateway(innerGateway, TimeSpan.FromMinutes(30));
		});

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

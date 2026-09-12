# openTheWindows

A .NET MAUI app that answers one question: **is it a good time to open the
windows instead of running the AC?**

You give it your home's zip code and your personal comfort preferences
(temperature and dew point ranges). It checks the current outdoor conditions
from the free [National Weather Service API](https://www.weather.gov/documentation/services-web-api)
(US only, no API key required) and tells you whether conditions are comfortable
enough to open up.

Initial target: Windows desktop. Longer-term goal: a background check that
updates an iPadOS/iOS home-screen widget without the user needing to open the
app (note: a real iOS/iPadOS widget is implemented as a separate native
WidgetKit/SwiftUI extension that reads shared state - it can't run .NET code
directly, but the *decision logic* below is 100% reusable regardless of what
reads it).

## Solution layout

```
src/
  OpenTheWindows.Core/          Plain C# class library. No MAUI/UI dependencies.
    Models/                     Data types: Location, ComfortPreferences, WeatherReading, WeatherStation.
    Weather/                    Infrastructure: calls zippopotam.us (zip -> lat/lon) and
                                 api.weather.gov (nearby stations + latest observation).
    Exercises/                  <-- Where the hands-on learning happens. See below.
tests/
  OpenTheWindows.Core.Tests.XUnit/   Tests using xUnit v3 + Microsoft.Testing.Platform.
  OpenTheWindows.Core.Tests.TUnit/   The SAME tests using TUnit + Microsoft.Testing.Platform,
                                     so you can compare the two frameworks directly.
```

The `Exercises` folder is intentionally left as `NotImplementedException` stubs
with detailed doc-comment instructions - **write these yourself, without AI
assistance**, as a way to learn C#:

1. `LocationInput.ReadLocation` - prompt for and validate a US zip code.
2. `ComfortPreferencesInput.ReadPreferences` - prompt for and validate the four
   comfort numbers (min/max temperature, min/max dew point).
3. `ComfortEvaluator.ShouldOpenWindows` - the core decision: is the current
   reading within both comfortable ranges?
4. `WeatherStationSelector.SelectBestStation` - given several nearby weather
   stations NWS knows about, pick which one to actually query (closest wins).

Each test project has a couple of `[Fact]`/`[Test]` methods already written
as examples, plus several marked `Skip = "TODO..."` describing a scenario for
you to implement yourself as a quick win. Once you implement the matching
`Exercises` class, remove the `Skip` and fill in the test body.

## Running things

```powershell
dotnet build                                              # whole solution
dotnet test tests/OpenTheWindows.Core.Tests.XUnit         # xUnit v3 via MTP
dotnet test tests/OpenTheWindows.Core.Tests.TUnit         # TUnit via MTP
```

Both test projects will show real failures right now (`NotImplementedException`)
until the `Exercises` classes are filled in - that's expected; it's a
test-driven starting point.

## MAUI app head(s)

Not scaffolded yet - this machine's dotnet install needs the `maui` workload,
which requires an **elevated (Administrator)** terminal to install:

```powershell
dotnet workload install maui
```

Once installed, we'll add a `src/OpenTheWindows.App/` MAUI project (Windows
target first) referencing `OpenTheWindows.Core`. We're prototyping both a
native-XAML UI and a Blazor Hybrid UI before settling on one, since a real iOS
widget is native Swift regardless of this choice.

## Requirements

- .NET 10 SDK
- Free NWS API - no key needed, just a descriptive `User-Agent` (see
  `NwsWeatherGateway`) per NWS's usage policy.

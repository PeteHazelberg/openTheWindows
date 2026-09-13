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

## Getting your machine set up

Follow these steps to prepare your Windows development machine (both ARM64 and x64 are supported).

### 1. Install the .NET 10 SDK

You will need the .NET 10 SDK installed.

- **Via winget (recommended):**
  ```powershell
  winget install Microsoft.DotNet.SDK.10
  ```
- **Or manual installer:** Download the .NET 10 SDK from [dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0) (the installer will automatically detect whether your machine is ARM64 or x64).

### 2. Install the .NET MAUI workload

The MAUI workload installs the platform SDKs and templates for building desktop and mobile UI apps.

> **Important:** This command modifies `Program Files\dotnet` and **must be run in an elevated (Administrator) PowerShell or Windows Terminal**. If run without elevation, it will fail with an error like:
> `Workload installation failed: ... The operation was canceled by the user.`

Open PowerShell or Terminal **as Administrator** and run:

```powershell
dotnet workload install maui
```

*(Note: If you only plan to work on the core logic exercises and unit tests in `OpenTheWindows.Core`, the MAUI workload is not strictly required—only the .NET 10 SDK is needed.)*

### 3. Verify your environment

In a regular PowerShell prompt, verify that .NET 10 and the MAUI workload are recognized:

```powershell
dotnet --version            # Should display 10.0.xxx
dotnet --list-sdks          # Confirms the 10.0.xxx SDK path
dotnet workload list        # Should list 'maui' under Installed Workload Id
```

### 4. Set up your editor

You can use either Visual Studio 2026 or Visual Studio Code:

#### Option A: Visual Studio 2026 Community (Recommended)
1. Download and run the Visual Studio 2026 installer from [visualstudio.microsoft.com](https://visualstudio.microsoft.com/).
2. In the installer workloads tab, check **.NET Multi-platform App UI development**.
3. Complete installation. Opening `OpenTheWindows.slnx` will provide integrated XAML/Blazor editing, IntelliSense, test runners, and one-click debugging.

#### Option B: Visual Studio Code
1. Install [Visual Studio Code](https://code.visualstudio.com/).
2. Install the following extensions from the VS Code Marketplace:
   - **C# Dev Kit** (`ms-dotnettools.csdevkit`)
   - **.NET MAUI** (`ms-dotnettools.dotnet-maui`)
3. Open the `openTheWindows` repository folder. The Solution Explorer in C# Dev Kit will load the projects.

### 5. Clone the repository

```powershell
git clone https://github.com/PeteHazelberg/openTheWindows.git
cd openTheWindows
```

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

Two Windows-targeted prototypes exist so you can compare native XAML vs. Blazor
Hybrid before settling on one - both reference `OpenTheWindows.Core` and wire
up the same "check my zip code" flow:

```powershell
dotnet run --project src\OpenTheWindows.App.Native -f net10.0-windows10.0.19041.0
dotnet run --project src\OpenTheWindows.App.BlazorHybrid -f net10.0-windows10.0.19041.0
```

Both currently show "Waiting on the Exercises classes to be implemented!"
when you click the button, since `ComfortEvaluator`/`WeatherStationSelector`
are still `NotImplementedException` stubs - that's expected until the
`Exercises` classes are filled in.

Both are currently scoped to `net10.0-windows10.0.19041.0` only; Android/iOS/
MacCatalyst target frameworks can be added back to each `.csproj` once we're
ready to target mobile (remember: any real iOS/iPadOS widget will still need
a separate native Swift/WidgetKit extension regardless of which of these we
keep).

## Weather data source note

We use the free National Weather Service API (api.weather.gov). No API key or account is required, but NWS requests a descriptive `User-Agent` header identifying the application (this is already configured in `NwsWeatherGateway`).

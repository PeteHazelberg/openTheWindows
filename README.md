# openTheWindows

openTheWindows is a small .NET app that answers one question: is it comfortable enough outside to open the windows instead of running the AC?

The app collects a home location, reads the user’s comfort preferences, checks the current outdoor conditions, and decides whether the windows should be open. It starts as a Windows-first Blazor Hybrid app and keeps the core weather logic separate so it can later feed a widget or background service.

## Architecture at a glance

- `src/OpenTheWindows.Core/` holds the plain C# decision logic and weather infrastructure.
- `src/OpenTheWindows.App.BlazorHybrid/` is the Windows-first UI shell.
- `tests/OpenTheWindows.Core.Tests.TUnit/` holds the TUnit test project.
- `README.md` files inside each project explain that project’s purpose and tradeoffs.

## Target platform and runtime notes

This repo targets .NET 10 because it is the stable, beginner-friendly SDK choice for a new project. The app is intentionally designed to be AOT-conscious without making NativeAOT a day-one requirement.

We keep the door open to NativeAOT by favoring explicit code, source-generated JSON, and minimal reflection-heavy runtime behavior in infrastructure code. Simplicity still wins when that would make the project harder for a beginner to understand.

## Getting your machine set up

### 1. Install the .NET 10 SDK

Use the official .NET 10 SDK on your Windows machine.

```powershell
winget install Microsoft.DotNet.SDK.10
```

If you prefer a manual install, download it from:
https://dotnet.microsoft.com/download/dotnet/10.0

### 2. Install the MAUI workload

The Blazor Hybrid app is built on MAUI, so install the workload in an Administrator PowerShell window.

```powershell
dotnet workload install maui
```

This command touches the .NET installation under `Program Files\dotnet`, so it must be run elevated. If you are only working on the pure core logic and tests, the SDK alone is enough; the MAUI workload is needed for the app shell.

### 3. Verify the toolchain

```powershell
dotnet --version
dotnet --list-sdks
dotnet workload list
```

You should see a 10.0.x SDK and the `maui` workload installed.

### 4. Install your editor

You can use either Visual Studio 2026 or VS Code.

#### Visual Studio 2026 (recommended)

- Install Visual Studio 2026 Community.
- In the installer, include the `.NET Multi-platform App UI development` workload.
- Open `OpenTheWindows.slnx` in the repo.

#### VS Code

Install the following extensions:

- `ms-dotnettools.csdevkit`
- `ms-dotnettools.dotnet-maui`

Then open the repository root folder.

## Solution layout

```text
src/
  OpenTheWindows.Core/
  OpenTheWindows.App.BlazorHybrid/

tests/
  OpenTheWindows.Core.Tests.TUnit/
```

The project intentionally keeps the UI thin and the decision logic separate from the app shell. This is a better fit for a beginner project and a better foundation for future widget/background work.

## Weather source and cache policy

This repo uses the free National Weather Service API (`api.weather.gov`) and does not require an API key. The app also has a small file-backed cache wrapper that stores recent weather observations locally and refreshes them after a configurable window (default: 30 minutes).

This avoids repeatedly hitting the external weather service during local development, unit testing, and repeated user checks.

## Building and testing

```powershell
dotnet build
dotnet run --project tests\OpenTheWindows.Core.Tests.TUnit
```

The test project contains focused scenarios for beginner learning. Some of those tests may remain intentionally skipped or incomplete while the student writes the matching logic.

## Running the app

```powershell
dotnet run --project src\OpenTheWindows.App.BlazorHybrid -f net10.0-windows10.0.19041.0
```

## CI workflow

The repository includes a GitHub Actions build/test workflow that runs on pushes and pull requests to `main`. The workflow builds `OpenTheWindows.Core` and the TUnit test project (not the MAUI app, since the CI runner doesn't have the MAUI workload installed) and requires the tests to pass before merging.

Right now all 8 tests in the TUnit project are `[Skip]`-ped or intentionally not yet implemented, since they describe beginner exercises in `src/OpenTheWindows.Core/Exercises/`. As those `Exercises` classes are implemented, remove the matching `[Skip]` attributes so the tests start running for real.

## Beginner learning goals

The project is intentionally structured so a beginner can make progress in small, clear steps:

1. Read the zip code and validate it.
2. Read comfort preferences.
3. Decide whether the temperature and dew point are comfortable.
4. Select the best nearby weather station.
5. Write the unit tests that describe the intended behavior.

The `Exercises/` folder is where the human learning work belongs. It is intentionally kept simple and explicit.

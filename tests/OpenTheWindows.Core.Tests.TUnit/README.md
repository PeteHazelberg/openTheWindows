# OpenTheWindows.Core.Tests.TUnit

This project is the repository’s chosen unit test project. It uses TUnit with Microsoft Testing Platform because it fits the project’s goals: simple test code, explicit assertions, and a source-generated testing model that is easier to keep friendly to future NativeAOT work.

## Why TUnit

The project is deliberately beginner-friendly. TUnit is readable, direct, and friendly to a teaching workflow where students can add small tests and understand the exact scenario being checked.

## How this project is used

This project has two kinds of tests:

- `Exercises/` tests are intentionally small teaching scenarios for the beginner.
- `Infrastructure/` tests are fully written, source-generated, and explain how the weather and geocoding plumbing is meant to behave.

The infrastructure tests are the ones to read first when you want to understand how the app collects, interprets, and caches weather data. The `Exercises/` tests are the human’s assignment, and they may remain skipped while the beginner writes the matching logic.

## The exercise tests

`ShouldOpenWindows_WhenTemperatureAndDewPointAreComfortable_ReturnsTrue` and
`SelectBestStation_WithOneCandidate_ReturnsIt` are fully-written example tests,
but they are currently marked `[Skip]` because they call the
`Exercises/ComfortEvaluator.cs` and `Exercises/WeatherStationSelector.cs`
methods, which are still `NotImplementedException` stubs. This keeps the CI
build/test gate green in the meantime. **Once those two `Exercises` classes
are implemented, remove the `[Skip]` from these two example tests** - they
should then pass without any other changes.

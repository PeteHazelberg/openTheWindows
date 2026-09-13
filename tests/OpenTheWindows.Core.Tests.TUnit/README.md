# OpenTheWindows.Core.Tests.TUnit

This project is the repository’s chosen unit test project. It uses TUnit with Microsoft Testing Platform because it fits the project’s goals: simple test code, explicit assertions, and a source-generated testing model that is easier to keep friendly to future NativeAOT work.

## Why TUnit

The project is deliberately beginner-friendly. TUnit is readable, direct, and friendly to a teaching workflow where students can add small tests and understand the exact scenario being checked.

## How this project is used

The tests exercise the logic in the `Exercises/` folder and are meant to guide a beginner through small wins:

- comfortable conditions return true
- too-hot or too-cold conditions return false
- high dew point does not qualify
- boundary values are handled consistently

These tests are intentionally used as teaching examples and may remain skipped while the beginner writes the matching logic.

## Temporary skips on the two "EXAMPLE" tests

`ShouldOpenWindows_WhenTemperatureAndDewPointAreComfortable_ReturnsTrue` and
`SelectBestStation_WithOneCandidate_ReturnsIt` are fully-written example tests,
but they are currently marked `[Skip]` too, because they call the
`Exercises/ComfortEvaluator.cs` and `Exercises/WeatherStationSelector.cs`
methods, which are still `NotImplementedException` stubs. This keeps the CI
build/test gate green in the meantime. **Once those two `Exercises` classes
are implemented, remove the `[Skip]` from these two example tests** - they
should then pass without any other changes.

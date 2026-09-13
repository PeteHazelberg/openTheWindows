# OpenTheWindows.Core

This project is the plain C# heart of the app. It contains the data models, weather integration, and the deliberately simple logic the project uses to decide whether the weather is comfortable enough to open the windows.

## Why this project exists

The app is designed to be beginner friendly. The core library stays free of UI dependencies and is intentionally explicit: plain classes, straightforward methods, and no hidden runtime magic. That makes it easier to teach, easier to debug, and easier to move toward NativeAOT later without reworking the whole design.

## Design choices

- Plain C# types only: the models are simple data objects that are easy to read in a Java-like style.
- Weather infrastructure is separate from the exercise logic: the API calls and JSON conversion live under `Weather/` and are not part of the beginner learning tasks.
- Source-generated JSON is preferred for AOT-friendly infrastructure, keeping the future NativeAOT path open.
- The `Exercises/` folder is intentionally left as learning work for a beginner and is not modified by AI tooling as part of routine implementation.

## Scope

The core project includes:

- `Models/`: the `Location`, `ComfortPreferences`, `WeatherReading`, and `WeatherStation` types.
- `Weather/`: the free NWS/API logic, geocoding, and the file-backed cache wrapper.
- `Exercises/`: the beginner tasks, left deliberately as stubs with instructions.

## Keep it simple

The project tries to stay readable for someone new to programming. When in doubt, choose a direct `if`, a clear method name, and a small amount of code over a clever abstraction.

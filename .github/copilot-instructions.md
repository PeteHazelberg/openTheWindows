# Copilot instructions for openTheWindows

These are project-specific instructions for any AI assistant (including
GitHub Copilot) working in this repository. Read this before making
changes.

## What this project is

A .NET MAUI app (Blazor Hybrid UI) that decides whether it's a good time to
open the windows instead of running the AC, based on outdoor temperature and
dew point from the free National Weather Service API. See the root
`README.md` and each project's own `README.md` for details and design
rationale.

## Top priority: simplicity and explicitness for a beginner

This repository is a learning project for a high-school-age beginner
programmer (coming from Java). **This is the most important rule in this
file and overrides the NativeAOT-friendliness guidance below whenever they
conflict.**

- Prefer plain, explicit C# that reads like the Java he already knows:
  regular classes, explicit types, straightforward `if`/`for`/`foreach`,
  simple methods.
- Avoid "clever" or terse modern C# shortcuts in code he is expected to read
  or write himself - this includes most LINQ chains, pattern-matching-heavy
  `switch` expressions, records used as behavior-bearing types, and deeply
  nested expression bodies. A few straightforward LINQ calls (`.Select`,
  `.Where`, `.FirstOrDefault`) are fine when they're clearer than a loop, but
  don't chain many together or use them to be "clever."
- **Never modify anything in `Exercises/` folders** (currently
  `src/OpenTheWindows.Core/Exercises/`). Those are intentionally
  `NotImplementedException` stubs meant to be implemented by the human,
  without AI assistance. Do not implement them, do not "helpfully" fix them,
  and do not change their doc comments/instructions unless explicitly asked.
- When asked to explain code in this repo, prefer plain language and short
  examples over dense technical jargon.

## NativeAOT-friendliness (secondary priority, applied everywhere else)

We want to keep the door open to publishing this app with NativeAOT in the
future. Outside of the `Exercises/` folders (and outside of code a beginner
is expected to read/write), prefer patterns that work well with NativeAOT
and trimming:

- **Use source-generated JSON serialization.** Any type that gets
  serialized/deserialized with `System.Text.Json` should have a
  `JsonSerializerContext` (see `Weather/WeatherJsonModels.cs` for the
  existing pattern) instead of relying on runtime reflection-based
  serialization. Do not use `JsonSerializer.Serialize<T>()` /
  `Deserialize<T>()` overloads that rely on reflection when a source-generated
  context is available or reasonably easy to add.
- **Avoid runtime reflection** in infrastructure code: no
  `Activator.CreateInstance`, no reflection-based DI container magic beyond
  what MAUI's built-in `MauiAppBuilder`/`IServiceCollection` already provides,
  no dynamic proxies.
- **Be cautious with third-party libraries.** Before adding a new NuGet
  package for infrastructure code, consider whether it's known to be
  trim/AOT-safe (check for `IsTrimmable`/`IsAotCompatible` package metadata,
  or recent release notes mentioning trimming/NativeAOT support). Prefer
  well-maintained libraries with explicit AOT support over older or
  reflection-heavy ones. When in doubt, ask before adding a new dependency.
- **Test frameworks are exempt.** `TUnit` was specifically chosen for its
  source-generated (non-reflection-based) test discovery, which is why it
  replaced xUnit in this repo - see
  `tests/OpenTheWindows.Core.Tests.TUnit/README.md`. Test project code
  itself does not need to be published with NativeAOT, so this constraint
  is about the app's own runtime code, not test tooling.
- Prefer `sealed` classes and explicit constructors over patterns that rely
  on runtime code generation.

If a NativeAOT-friendly approach would be significantly harder to explain or
use than a simpler alternative, and the code in question isn't part of the
performance-critical or frequently-published path, **default to the simpler
approach** and leave a short comment noting the tradeoff. We are not
NativeAOT-publishing yet - this is about not painting ourselves into a
corner, not about optimizing prematurely.

## Documentation pattern

Every project folder should have its own `README.md` describing that
project's specific philosophy and design decisions (why it exists, why it's
built the way it is), separate from the root `README.md`, which stays
focused on the whole-repo overview, setup instructions, and how to run
things. When you add a new project, add a matching `README.md` for it.

## Weather API usage

This project only targets the United States and uses the free National
Weather Service API (`api.weather.gov`), which requires no API key but does
require a descriptive `User-Agent` header (see `NwsWeatherGateway`). Do not
introduce a different weather provider without discussing it first - the
free/US-only/no-key constraint was a deliberate choice.

## Before committing

- Run `dotnet build` and `dotnet run --project tests\OpenTheWindows.Core.Tests.TUnit`
  and confirm they succeed before considering a change complete.
- Keep changes scoped to what was asked; don't restructure unrelated code
  "while you're in there."

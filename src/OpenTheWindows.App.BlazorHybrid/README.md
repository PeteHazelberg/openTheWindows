# OpenTheWindows.App.BlazorHybrid

This project is the user-facing app shell for the project. It keeps the UI thin and lets the Core library do the weather and decision logic.

## Why Blazor Hybrid

We are starting with a Windows desktop app, and Blazor Hybrid gives us a fast path to a working UI without the overhead of a fully native app shell. It is easy to reason about, easy to prototype, and keeps the app’s business logic in the shared Core project rather than mixing it into UI code.

## Architecture

- The UI is intentionally minimal: a simple form for zip code and comfort ranges, and a button to check whether the windows should be open.
- The app reads from the Core library instead of embedding decision logic in components.
- Weather access goes through a small cache decorator so repeated checks during local development do not keep re-fetching the external NWS API.

## Future direction

This is a good stepping stone toward a widget-friendly design. The decision logic and weather read model stay in the Core project so later work can feed a widget or background service without rewriting the rules.

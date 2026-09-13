# Infrastructure tests

These tests are intentionally fully written. They are meant to help someone new to the codebase understand how the weather plumbing works without having to read every implementation detail by hand.

Read these when you are learning the app’s inner workings:

- `GeoMathTests` explains the distance math behind station selection.
- `NwsWeatherGatewayTests` shows how the app turns National Weather Service JSON into real `WeatherStation` and `WeatherReading` values.
- `ZippopotamGeocodingServiceTests` shows how a zip code becomes latitude and longitude.
- `CachingWeatherGatewayTests` explains the file-backed observation cache, how expiration works, and why we guard it with a lock.

The `Exercises/` folder remains intentionally blank or stubbed so a student can work through the logic themselves.

using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Exercises;

/// <summary>
/// EXERCISE (for the human, not AI-assisted!): the NWS API can return several
/// candidate weather stations near a home. Pick the one we should actually query.
///
/// Requirements:
///  - candidates will never be empty when this is called in the real app, but your
///    code should still behave reasonably (e.g. throw ArgumentException) if it is.
///  - Choose the station with the smallest DistanceMiles - i.e. the closest one to
///    the residence. That's usually the most representative reading for "should I
///    open my windows".
///  - This is similar to picking the "best" sensor reading in robotics code - when
///    you have several data sources, you pick the one you trust most for the job.
/// </summary>
public static class WeatherStationSelector
{
    public static WeatherStation SelectBestStation(IReadOnlyList<WeatherStation> candidates)
    {
        throw new NotImplementedException("TODO: implement WeatherStationSelector.SelectBestStation");
    }
}

using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Exercises;

/// <summary>
/// EXERCISE (for the human, not AI-assisted!): Ask the user for their zip code
/// and turn it into a <see cref="Location"/>.
///
/// Requirements:
///  - Prompt the user (write something to <paramref name="output"/>) asking for a 5-digit US zip code.
///  - Read one line from <paramref name="input"/>.
///  - A valid zip code is exactly 5 digits (e.g. "80202"). Anything else (letters, wrong
///    length, blank) is invalid.
///  - If the input is invalid, print a helpful message and ask again - keep looping until
///    you get a valid zip code.
///  - Return a new Location with just the ZipCode set (Latitude/Longitude come later,
///    from the geocoding service - you don't need to set them here).
///
/// Why TextReader/TextWriter instead of Console.ReadLine()/Console.WriteLine()?
/// It lets unit tests feed in fake input (a StringReader) and capture output (a
/// StringWriter) without needing a real keyboard/screen. When you actually run the
/// app you'll pass in Console.In and Console.Out.
/// </summary>
public static class LocationInput
{
    public static Location ReadLocation(TextReader input, TextWriter output)
    {
        throw new NotImplementedException("TODO: implement LocationInput.ReadLocation");
    }
}

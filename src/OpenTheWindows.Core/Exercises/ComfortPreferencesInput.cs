using OpenTheWindows.Core.Models;

namespace OpenTheWindows.Core.Exercises;

/// <summary>
/// EXERCISE (for the human, not AI-assisted!): Ask the user for their comfort
/// preferences and turn them into a <see cref="ComfortPreferences"/>.
///
/// Requirements:
///  - Prompt for, and read, four numbers (each on its own line is fine):
///      1. Minimum comfortable temperature (F)
///      2. Maximum comfortable temperature (F)
///      3. Minimum comfortable dew point (F)
///      4. Maximum comfortable dew point (F)
///  - Each value must parse as a number (see float.TryParse). If it doesn't, print a
///    message and ask again for that value.
///  - Each "Min" must be less than or equal to its matching "Max". If not, print a
///    message explaining the problem and ask again for both values.
///  - Return a ComfortPreferences with all four values set.
///
/// Hint: consider writing yourself a small private helper method that reads and
/// validates a single number, so you're not copy/pasting the same loop four times.
/// </summary>
public static class ComfortPreferencesInput
{
    public static ComfortPreferences ReadPreferences(TextReader input, TextWriter output)
    {
        throw new NotImplementedException("TODO: implement ComfortPreferencesInput.ReadPreferences");
    }
}

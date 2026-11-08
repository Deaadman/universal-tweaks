namespace UniversalTweaks.Core;

/// <summary>The boxes of rounds that have a single-round item to drop instead.</summary>
internal static class AmmoRounds
{
    /// <summary>The suffix Unity adds to an instantiated object's name.</summary>
    private const string CloneSuffix = "(Clone)";

    /// <summary>The single round for each box of rounds, by gear item name.</summary>
    private static readonly Dictionary<string, string> SingleRounds = new()
    {
        { "GEAR_RifleAmmoBox", "GEAR_RifleAmmoSingle" },
        { "GEAR_RevolverAmmoBox", "GEAR_RevolverAmmoSingle" }
    };

    /// <summary>Gets the single round to drop instead of a box holding this many rounds.</summary>
    /// <param name="gearItemName">The dropped item's name, like <c>GEAR_RifleAmmoBox</c>, with or without <c>(Clone)</c>.</param>
    /// <param name="units">How many rounds are being dropped.</param>
    /// <returns>
    /// The single round's gear item name, or <see langword="null"/> unless it's one round from a box that has a
    /// single-round item.
    /// </returns>
    internal static string? GetSingleRound(string gearItemName, int units)
    {
        if (units != 1)
            return null;

        if (gearItemName.EndsWith(CloneSuffix, StringComparison.Ordinal))
            gearItemName = gearItemName[..^CloneSuffix.Length].TrimEnd();

        return SingleRounds.TryGetValue(gearItemName, out var singleRoundName) ? singleRoundName : null;
    }
}

using UniversalTweaks.Core;

namespace UniversalTweaks.Utilities;

/// <summary>
/// Drops one round split off a box of rounds as a single round instead of a box, when
/// <see cref="Settings.DropSingleRounds"/> is on.
/// </summary>
/// <remarks>
/// The game splits the dropped rounds off into a new item, then places it. The new item is swapped as it's made, before
/// the game places it, so the round lands where the box would have. Only drops are changed; moving rounds into a
/// container keeps the game's behaviour.
/// </remarks>
internal static class SingleRoundDrops
{
    /// <summary>Whether the player is dropping an item, so splits are for a drop.</summary>
    internal static bool Dropping { get; set; }

    /// <summary>Swaps a newly split-off box holding one round for a single round, when dropping.</summary>
    /// <param name="split">The item the game split off.</param>
    /// <param name="units">How many rounds it holds.</param>
    /// <returns>The single round, or <paramref name="split"/> if it's left as it is.</returns>
    internal static GearItem? SwapForSingleRound(GearItem? split, int units)
    {
        if (!Dropping || !Settings.Instance.DropSingleRounds || split == null)
            return split;

        var singleRoundName = AmmoRounds.GetSingleRound(split.name, units);
        if (singleRoundName == null)
            return split;

        var single = GearItem.InstantiateGearItem(singleRoundName);
        if (single == null || single.m_StackableItem == null)
        {
            if (single != null)
                UnityEngine.Object.Destroy(single.gameObject);

            Mod.Logger.Log($"Couldn't make '{singleRoundName}'; dropping the box instead.", FlaggedLoggingLevel.Warning);
            return split;
        }

        DisableObjectForXPMode.RemoveDisabler(single.gameObject);
        single.m_StackableItem.m_Units = 1;
        single.SetNormalizedHP(split.GetNormalizedCondition(), false);
        single.m_BeenInspected = split.m_BeenInspected;

        var singleTransform = single.transform;
        var splitTransform = split.transform;
        singleTransform.SetParent(splitTransform.parent, false);
        singleTransform.SetPositionAndRotation(splitTransform.position, splitTransform.rotation);
        single.gameObject.SetActive(split.gameObject.activeSelf);

        UnityEngine.Object.Destroy(split.gameObject);

        Mod.Logger.Log($"Dropped '{singleRoundName}' instead of '{split.name}'.", FlaggedLoggingLevel.Debug);
        return single;
    }
}
